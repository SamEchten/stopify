namespace Stopify.DTO.Music;

public class AlbumDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<SongDTO> Songs { get; set; } = [];
}