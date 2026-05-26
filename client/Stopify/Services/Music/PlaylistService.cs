using System.Net.Http.Json;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public class PlaylistService(HttpClient http) : IPlaylistService
{
    public async Task<List<Playlist>> GetUserPlaylistsAsync()
    {
        var playlists = await http.GetFromJsonAsync<List<Playlist>>("/api/playlists/get-by-user");
        return playlists ?? [];
    }

    public async Task CreatePlaylistAsync(string title)
    {
        var response = await http.PostAsync($"/api/playlists?title={Uri.EscapeDataString(title)}", null);
        response.EnsureSuccessStatusCode();
    }

}
