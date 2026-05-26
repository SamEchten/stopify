using Stopify.Models.Music;

namespace Stopify.Services.Music;

public interface IPlaylistService
{
    Task<List<Playlist>> GetUserPlaylistsAsync();
    Task CreatePlaylistAsync(string title);
}
