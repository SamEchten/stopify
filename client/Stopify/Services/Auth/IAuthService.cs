namespace Stopify.Services.Auth;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task RegisterUserAsync(string username, string email, string password);
    Task RegisterArtistAsync(string username, string artistName, string email, string password);
    Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
}
