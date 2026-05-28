namespace Stopify.Requests.Music;

public class CreateAlbumRequest(string title)
{
    public string Title { get; } = title;
}
