﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stopify.Entities.Auth;
using Stopify.Entities.Users;
using Stopify.Repositories.Auth;
using Stopify.Repositories.Users;

namespace Stopify.Services.Auth;

public class AuthService(
    IOptions<AppSettings> appSettings,
    UserRepository userRepository,
    HashingService hashingService,
    RefreshTokenRepository refreshTokenRepository
    ) : IService
{
    public User? Login(string? username, string? email, string password)
    {
        var user = username != null
            ? userRepository.GetByUsername(username)
            : userRepository.GetByEmail(email!);

        if (user == null)
        {
            return null;
        }

        return user.Password == hashingService.HashPassword(password) ? user : null;
    }

    public string GenerateAccessToken(int userId, int expireMinutes = 30)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(appSettings.Value.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = appSettings.Value.Issuer,
            Audience = appSettings.Value.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(User user)
    {
        var token = Guid.NewGuid().ToString();

        var refreshToken = RefreshTokenFactory.Build(token, user);

        refreshTokenRepository.Add(refreshToken);

        return refreshToken;
    }

    public RefreshToken FindOrGenerateRefreshToken(User user)
    {
        var existingToken = refreshTokenRepository.GetByUser(user);

        if (existingToken == null || existingToken.ExpiresAt < DateTime.UtcNow) return GenerateRefreshToken(user);

        return existingToken;
    }

    public bool ValidateRefreshToken(string token)
    {
        var existingToken = refreshTokenRepository.GetByToken(token);

        return existingToken != null && existingToken.ExpiresAt >= DateTime.UtcNow;
    }

    public RefreshToken? GetNewOrExistingRefreshToken(string token)
    {
        var existingToken = refreshTokenRepository.GetByToken(token);

        if (existingToken == null) return null;

        return existingToken.ExpiresAt < DateTime.UtcNow ? GenerateRefreshToken(existingToken.User) : existingToken;
    }
}