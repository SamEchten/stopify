using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Attribute.Auth;
using Stopify.Entities.Music;
using Stopify.Repositories.Music;
using Stopify.Requests.Music;
using Stopify.Services.Music;

namespace Stopify.Controllers.Music;

[Authorize]
[ApiController]
[Route("/api/songs")]
public class SongController(SongService songService, SongRepository songRepository) : ControllerBase
{
    [AuthorizeArtist]
    [HttpPost("upload", Name = "UploadSong")]
    public ActionResult UploadSong([FromForm] UploadSongRequest request)
    {
        var song = songService.UploadSong(request.SongName, request.ArtistIds, request.file);

        return Ok(song);
    }

    [HttpGet(Name = "GetSongs")]
    public ActionResult<ICollection<Song>> GetSongs()
    {
        var songs = songRepository.GetAll();

        return Ok(songs);
    }

    [HttpGet("{songId:int}", Name = "GetSong")]
    public ActionResult<Song> GetSong(int songId)
    {
        var song = songRepository.GetById(songId);

        if (song == null) return NotFound();

        return Ok(song);
    }
}