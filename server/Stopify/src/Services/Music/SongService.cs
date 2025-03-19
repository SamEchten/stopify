using Stopify.Entities.Music;
using Stopify.Repositories.Music;
using Stopify.Repositories.Users;
using Stopify.Services.Storage;

namespace Stopify.Services.Music;

public class SongService(
    StorageService storage,
    SongFactory songFactory,
    ArtistRepository artistRepository,
    SongRepository songRepository,
    ILogger<SongService> logger
) : IService {
    private readonly string _songsLocation = Path.Combine(Directory.GetCurrentDirectory(), "songs");
    public Song UploadSong(string songName, ICollection<int> artistIds, IFormFile file)
    {
        var fileLocation = storage.UploadFile(file, _songsLocation);

        logger.LogInformation("Uploaded song {songName} to {fileLocation}", songName, fileLocation);

        var artists = artistIds
            .Select(artistId => artistRepository.GetById(artistId) ?? throw new Exception($"Artist with id: {artistId} not found"))
            .ToList();

        var song = songFactory.Create(songName, artists, fileLocation);

        songRepository.Add(song);

        return song;
    }
}
