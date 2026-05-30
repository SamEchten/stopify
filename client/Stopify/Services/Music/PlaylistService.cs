using System.Net.Http.Json;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public class PlaylistService(HttpClient http) : IPlaylistService
{
    public event Action? PlaylistsChanged;

    public async Task<List<Playlist>> GetUserPlaylistsAsync()
    {
        var playlists = await http.GetFromJsonAsync<List<Playlist>>("/api/playlists/get-by-user");
        return playlists ?? [];
    }

    public async Task CreatePlaylistAsync(string title)
    {
        var response = await http.PostAsync($"/api/playlists?title={Uri.EscapeDataString(title)}", null);
        response.EnsureSuccessStatusCode();
        PlaylistsChanged?.Invoke();
    }

    public async Task<Playlist?> GetPlaylistAsync(int id)
    {
        return await http.GetFromJsonAsync<Playlist>($"/api/playlists/{id}");
    }

    public async Task AddSongToPlaylistAsync(int playlistId, int songId)
    {
        var response = await http.PutAsync($"/api/playlists/{playlistId}/{songId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSongFromPlaylistAsync(int playlistId, int songId)
    {
        var response = await http.PostAsync($"/api/playlists/delete-song/{playlistId}/{songId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePlaylistAsync(int playlistId)
    {
        var response = await http.DeleteAsync($"/api/playlists/{playlistId}");
        response.EnsureSuccessStatusCode();
        PlaylistsChanged?.Invoke();
    }
}
