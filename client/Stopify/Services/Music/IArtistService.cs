using Stopify.Models.Music;

namespace Stopify.Services.Music;

public interface IArtistService
{
    Task<List<Artist>> GetAllArtistsAsync();
    Task<List<Song>> GetArtistSongsAsync(int artistId);
    Task<List<Album>> GetArtistAlbumsAsync(int artistId);
}
