using Stopify.Entities.Music;
using Stopify.Entities.Users;
using Stopify.Repositories.Music;

namespace Stopify.Services.Music;

public class AlbumService(AlbumRepository albumRepository, AlbumFactory albumFactory, SongRepository songRepository) : IService
{
    public Album CreateAlbum(string title, Artist artist)
    {
        var album = albumFactory.Create(title, artist);
        albumRepository.Add(album);
        return album;
    }

    public bool DeleteAlbum(int albumId, int artistId)
    {
        var album = albumRepository.GetById(albumId);
        if (album == null || album.Artist.Id != artistId) return false;
        albumRepository.Delete(albumId);
        return true;
    }

    public bool AddSongToAlbum(int albumId, int songId, int artistId)
    {
        var album = albumRepository.GetById(albumId);
        if (album == null || album.Artist.Id != artistId) return false;

        var song = songRepository.Find(songId);
        if (song == null) return false;

        song.AlbumId = albumId;
        songRepository.Save();
        return true;
    }
}
