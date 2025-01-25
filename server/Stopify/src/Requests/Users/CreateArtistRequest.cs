namespace Stopify.Requests.Users;

public class CreateArtistRequest
{
    public string Username { get; set; }
    public string ArtistName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
