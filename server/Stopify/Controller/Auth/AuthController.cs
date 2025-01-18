using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Service.Auth;
using LoginRequest = Stopify.Request.Auth.LoginRequest;

namespace Stopify.Controller.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService): ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    public ActionResult Login([FromBody] LoginRequest request)
    {
        var user = authService.Login(request.Username, request.Email, request.Password);

        if (user == null) return BadRequest();

        var accessToken = authService.GenerateAccessToken(user.Id);
        var refreshToken = authService.FindOrGenerateRefreshToken(user);

        Response.Cookies.Append("Stopify-AccessToken", accessToken);
        Response.Cookies.Append("Stopify-RefreshToken", refreshToken.Token);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("refresh-token", Name="RefreshToken")]
    public ActionResult RefreshToken()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var token  = Request.Cookies["Stopify-RefreshToken"];

        if (string.IsNullOrEmpty(token)) return Unauthorized(new {message = "Refresh token is missing"});

        var refreshToken = authService.GetNewOrExistingRefreshToken(token);

        if (refreshToken == null) return Unauthorized(new {message = "Refresh token is invalid"});

        if (refreshToken.User.Id.ToString() != userId) return Unauthorized(new {message = "Invalid tokens provided"});

        var accessToken = authService.GenerateAccessToken(refreshToken.User.Id);

        Response.Cookies.Append("Stopify-AccessToken", accessToken);
        Response.Cookies.Append("Stopify-RefreshToken", refreshToken.Token);

        return Ok();
    }
}
