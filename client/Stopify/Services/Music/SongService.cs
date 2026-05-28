using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public class SongService(HttpClient http) : ISongService
{
    public async Task<List<Song>> GetAllSongsAsync()
    {
        return await http.GetFromJsonAsync<List<Song>>("/api/songs") ?? [];
    }

    public async Task<List<Song>> GetRecentSongsAsync()
    {
        var songs = await http.GetFromJsonAsync<List<Song>>("/api/songs");
        return songs?.OrderByDescending(s => s.Id).Take(10).ToList() ?? [];
    }

    public async Task<List<Song>> SearchSongsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];
        var songs = await http.GetFromJsonAsync<List<Song>>($"/api/songs/search?q={Uri.EscapeDataString(query)}");
        return songs ?? [];
    }

    public async Task<Song?> UploadSongAsync(string songName, IBrowserFile file, int artistId)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(songName), "SongName");
        content.Add(new StringContent(artistId.ToString()), "ArtistIds");
        var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 50_000_000));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/mpeg");
        content.Add(fileContent, "file", file.Name);

        var response = await http.PostAsync("/api/songs/upload", content);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<Song>();
    }
}
