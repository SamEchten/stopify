namespace Stopify.Entities.Users.DTO;

public class UserProfileDTO(int id, string username, string email, ICollection<string> roles, DateTime createdAt)
{
    public int Id { get; } = id;
    public string Username { get; } = username;
    public string Email { get; } = email;
    public ICollection<string> Roles { get; } = roles;
    public DateTime CreatedAt { get; } = createdAt;
}
