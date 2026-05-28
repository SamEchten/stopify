using System.Net.Http.Json;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public class AlbumService(HttpClient http) : IAlbumService
{
    public async Task<Album?> CreateAlbumAsync(string title)
    {
        var response = await http.PostAsJsonAsync("/api/albums", new { title });
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<Album>();
    }

    public async Task<bool> DeleteAlbumAsync(int albumId)
    {
        var response = await http.DeleteAsync($"/api/albums/{albumId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddSongToAlbumAsync(int albumId, int songId)
    {
        var response = await http.PutAsync($"/api/albums/{albumId}/songs/{songId}", null);
        return response.IsSuccessStatusCode;
    }
}
