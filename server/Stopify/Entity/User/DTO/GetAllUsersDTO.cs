namespace Stopify.Entity.User.DTO;

public class GetAllUsersDTO(int id, string username)
{
    public int Id { get; } = id;
    public string Username { get; } = username;
}