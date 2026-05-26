using System.Net.Http.Json;
using Stopify.Models.Auth;

namespace Stopify.Services.Auth;

public class AuthService(HttpClient http) : IAuthService
{
    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await http.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Email = email,
            Password = password
        });

        return response.IsSuccessStatusCode;
    }
}
