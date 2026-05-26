namespace Stopify.Services.Auth;

public interface IAuthStateService
{
    bool IsAuthenticated { get; }
    void SetAuthenticated(bool value);
    event Action OnChange;
}
