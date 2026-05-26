namespace Stopify.Models.Music;

public class Playlist
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Song> Songs { get; set; } = [];
}
