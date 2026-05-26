namespace Stopify.Services.Auth;

public class AuthStateService : IAuthStateService
{
    public bool IsAuthenticated { get; private set; }
    public event Action? OnChange;

    public void SetAuthenticated(bool value)
    {
        IsAuthenticated = value;
        OnChange?.Invoke();
    }
}
