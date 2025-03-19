namespace Stopify.DTO.Music;

public class SongDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FileLocation { get; set; }
    public List<ArtistDTO> Artists { get; set; }
}
