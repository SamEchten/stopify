namespace Stopify.Services.Auth;

public interface IAuthStateService
{
    bool IsAuthenticated { get; }
    int? UserId { get; }
    string? Username { get; }
    string? Email { get; }
    string? Role { get; }
    void SetAuthenticated(bool value);
    int? ArtistId { get; }
    void SetUserInfo(int userId, string username, string email, string role, int? artistId = null);
    void Clear();
    event Action OnChange;
}
