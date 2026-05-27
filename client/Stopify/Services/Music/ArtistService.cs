using System.Net.Http.Json;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public class ArtistService(HttpClient http) : IArtistService
{
    public async Task<List<Artist>> GetAllArtistsAsync()
    {
        return await http.GetFromJsonAsync<List<Artist>>("/api/artists") ?? [];
    }
}
