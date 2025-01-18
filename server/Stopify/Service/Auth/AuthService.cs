using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stopify.Entity.Auth;
using Stopify.Repository.Auth;
using Stopify.Repository.User;
using Stopify.Service.User;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Service.Auth;

public class AuthService(
    IOptions<AppSettings> appSettings,
    UserRepository userRepository,
    HashingService hashingService,
    RefreshTokenRepository refreshTokenRepository,
    RefreshTokenFactory refreshTokenFactory
    ) : IService
{
    public UserEntity? Login(string username, string password)
    {
        var user = userRepository.GetByUsername(username);
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

    private RefreshToken GenerateRefreshToken(UserEntity user)
    {
        var token = Guid.NewGuid().ToString();

        var refreshToken = refreshTokenFactory.Build(token, user);

        refreshTokenRepository.Add(refreshToken);

        return refreshToken;
    }

    public RefreshToken FindOrGenerateRefreshToken(UserEntity user)
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