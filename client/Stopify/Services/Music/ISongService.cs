using Microsoft.AspNetCore.Components.Forms;
using Stopify.Models.Music;

namespace Stopify.Services.Music;

public interface ISongService
{
    Task<List<Song>> GetRecentSongsAsync();
    Task<List<Song>> SearchSongsAsync(string query);
    Task<List<Song>> GetAllSongsAsync();
    Task<Song?> UploadSongAsync(string songName, IBrowserFile file, int artistId);
}
