namespace Stopify.Services.Auth;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
}
