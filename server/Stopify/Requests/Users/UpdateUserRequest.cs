namespace Stopify.Requests.Users;

public class UpdateUserRequest(string username, string email)
{
    public string Username { get; } = username;
    public string Email { get; } = email;
}