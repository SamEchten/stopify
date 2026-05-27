namespace Stopify.Models.Music;

public class Song
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FileLocation { get; set; } = string.Empty;
    public double Duration { get; set; }
    public List<Artist> Artists { get; set; } = [];
}
