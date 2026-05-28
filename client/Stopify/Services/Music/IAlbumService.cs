using Stopify.Models.Music;

namespace Stopify.Services.Music;

public interface IAlbumService
{
    Task<Album?> CreateAlbumAsync(string title);
    Task<bool> DeleteAlbumAsync(int albumId);
    Task<bool> AddSongToAlbumAsync(int albumId, int songId);
}
