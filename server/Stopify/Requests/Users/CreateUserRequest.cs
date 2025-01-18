namespace Stopify.Requests.Users;

public class CreateUserRequest(string username, string email, string password)
{
    public string Username { get; } = username;
    public string Email { get; } = email;
    public string Password { get; } = password;
}