using Stopify.Models.Music;

namespace Stopify.Services.Music;

public interface IPlaylistService
{
    event Action? PlaylistsChanged;
    Task<List<Playlist>> GetUserPlaylistsAsync();
    Task CreatePlaylistAsync(string title);
    Task<Playlist?> GetPlaylistAsync(int id);
    Task AddSongToPlaylistAsync(int playlistId, int songId);
    Task RemoveSongFromPlaylistAsync(int playlistId, int songId);
}
