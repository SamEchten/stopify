using System.Net.Http.Json;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public class ArtistService(HttpClient http) : IArtistService
{
    public async Task<List<Artist>> GetAllArtistsAsync()
    {
        return await http.GetFromJsonAsync<List<Artist>>("/api/artists") ?? [];
    }

    public async Task<List<Song>> GetArtistSongsAsync(int artistId)
    {
        return await http.GetFromJsonAsync<List<Song>>($"/api/artists/{artistId}/songs") ?? [];
    }

    public async Task<List<Album>> GetArtistAlbumsAsync(int artistId)
    {
        return await http.GetFromJsonAsync<List<Album>>($"/api/artists/{artistId}/albums") ?? [];
    }
}
