using Stopify.Models.Music;

namespace Stopify.Services.Music;

public interface IArtistService
{
    Task<List<Artist>> GetAllArtistsAsync();
}
