using Stopify.Models.Music;

namespace Stopify.Services.Music;

public interface ISongService
{
    Task<List<Song>> GetRecentSongsAsync();
}
