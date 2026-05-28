namespace Stopify.Services.Auth;

public class AuthStateService : IAuthStateService
{
    public bool IsAuthenticated { get; private set; }
    public int? UserId { get; private set; }
    public string? Username { get; private set; }
    public string? Email { get; private set; }
    public string? Role { get; private set; }
    public int? ArtistId { get; private set; }
    public event Action? OnChange;

    public void SetAuthenticated(bool value)
    {
        IsAuthenticated = value;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        IsAuthenticated = false;
        UserId = null;
        Username = null;
        Email = null;
        Role = null;
        ArtistId = null;
        OnChange?.Invoke();
    }

    public void SetUserInfo(int userId, string username, string email, string role, int? artistId = null)
    {
        UserId = userId;
        Username = username;
        Email = email;
        Role = role;
        ArtistId = artistId;
        OnChange?.Invoke();
    }
}
