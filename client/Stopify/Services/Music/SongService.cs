using System.Net.Http.Json;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public class SongService(HttpClient http) : ISongService
{
    public async Task<List<Song>> GetRecentSongsAsync()
    {
        var songs = await http.GetFromJsonAsync<List<Song>>("/api/songs");
        return songs?.OrderByDescending(s => s.Id).Take(10).ToList() ?? [];
    }

    public async Task<List<Song>> SearchSongsAsync(string query)
    {
        var songs = await http.GetFromJsonAsync<List<Song>>($"/api/songs/search?q={Uri.EscapeDataString(query)}");
        return songs ?? [];
    }
}
