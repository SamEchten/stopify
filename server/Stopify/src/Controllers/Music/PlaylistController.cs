using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Entities.Music;
using Stopify.Repositories.Music;
using Stopify.Repositories.Users;
using Stopify.Services.Music;

namespace Stopify.Controllers.Music;

[Authorize]
[ApiController]
[Route("/api/playlists")]
public class PlaylistController(PlaylistRepository playlistRepository, PlaylistFactory playlistFactory, UserRepository userRepository, SongRepository songRepository) : ControllerBase
{
    [HttpPost(Name = "CreatePlaylist")]
    public ActionResult<Playlist> CreatePlaylist(string title)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var user = userRepository.GetById(userId);
        if (user == null)
        {
            return Unauthorized($"User with given id {userId} not found");
        }

        var playlist = playlistFactory.CreatePlaylist(user, title);

        playlistRepository.Add(playlist);

        return Ok(playlist);
    }

    [HttpGet(Name = "GetPLaylists")]
    public ActionResult<ICollection<Playlist>> GetPlaylists()
    {
        var playlists = playlistRepository.GetAll();

        return Ok(playlists);
    }

    [HttpGet("get-by-user", Name = "GetPlaylistByUser")]
    public ActionResult<ICollection<Playlist>> GetPlaylistByUser()
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var user = userRepository.GetById(userId);

        var playlists = playlistRepository.GetByUser(user!);

        return Ok(playlists);
    }

    [HttpGet("{playlistId:int}", Name = "GetPLaylist")]
    public ActionResult<Playlist> GetPlaylist(int playlistId)
    {
        var playlist = playlistRepository.GetById(playlistId);

        if (playlist == null) return NotFound();

        return Ok(playlist);
    }

    [HttpPut("{playlistId:int}/{songId:int}", Name = "AddSong")]
    public ActionResult<Playlist> AddSong(int playlistId, int songId)
    {
        var song = songRepository.Find(songId);
        if (song == null)
        {
            return NotFound($"Song with id {songId} not found");
        }

        var playlist = playlistRepository.GetById(playlistId);
        if (playlist == null)
        {
            return NotFound($"Playlist with id {playlistId} not found");
        }

        playlist.AddSong(song);

        return playlist;
    }

    [HttpDelete("{playlistId:int}", Name = "RemoveSong")]
    public ActionResult DeletePlaylist(int playlistId)
    {
        var playlist = playlistRepository.Find(playlistId);
        if (playlist == null)
        {
            return NotFound($"Playlist with id {playlist} not found");
        }

        playlistRepository.Delete(playlistId);

        return Ok();
    }

    [HttpPost("delete-song/{playlistId:int}/{songId:int}", Name = "DeleteSong")]
    public ActionResult DeleteSong(int playlistId, int songId)
    {
        var playlist = playlistRepository.GetById(playlistId);
        if (playlist == null)
        {
            return NotFound($"Playlist with id {playlist} not found");
        }

        var song = songRepository.Find(songId);
        if (song == null)
        {
            return NotFound($"Song with id {song} not found");
        }

        playlist.DeleteSong(song);

        return Ok(playlist);
    }
}