namespace Stopify.Requests.Users;

public class UpdateUserRequest(int userId, string username, string email)
{
    public int UserId { get; } = userId;
    public string Username { get; } = username;
    public string Email { get; } = email;
}