using System.Net.Http.Json;
using Stopify.Models.Auth;
using Stopify.Models.Music;

namespace Stopify.Services.Auth;

public class AuthService(HttpClient http, IAuthStateService authState) : IAuthService
{
    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await http.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Email = email,
            Password = password
        });

        if (!response.IsSuccessStatusCode) return false;

        var profile = await http.GetFromJsonAsync<UserProfile>("/api/users/me");
        if (profile != null)
        {
            var role = profile.Roles.Contains("Artist") ? "Artist" : "User";
            int? artistId = null;
            if (role == "Artist")
            {
                var artist = await http.GetFromJsonAsync<Artist>("/api/artists/me");
                artistId = artist?.Id;
            }
            authState.SetUserInfo(profile.Id, profile.Username, profile.Email, role, artistId);
        }

        return true;
    }

    public async Task LogoutAsync()
    {
        await http.PostAsync("/api/auth/logout", null);
    }

    public async Task RegisterUserAsync(string username, string email, string password)
    {
        var content = new FormUrlEncodedContent([
            new("username", username),
            new("email", email),
            new("password", password)
        ]);
        var response = await http.PostAsync("/api/users", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task RegisterArtistAsync(string username, string artistName, string email, string password)
    {
        var content = new FormUrlEncodedContent([
            new("username", username),
            new("artistName", artistName),
            new("email", email),
            new("password", password)
        ]);
        var response = await http.PostAsync("/api/artists", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        var response = await http.PostAsJsonAsync("/api/users/change-password", new
        {
            currentPassword,
            newPassword
        });
        return response.IsSuccessStatusCode;
    }
}
