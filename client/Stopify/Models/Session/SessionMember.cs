namespace Stopify.Models.Session;

public class SessionMember
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public bool IsHost { get; set; }
}