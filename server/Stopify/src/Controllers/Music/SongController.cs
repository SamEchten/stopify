using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Attribute.Auth;
using Stopify.DTO.Music;
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
    public async Task<ActionResult> UploadSong([FromForm] UploadSongRequest request)
    {
        var song = await songService.UploadSong(request.SongName, request.ArtistIds, request.file);

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

    [HttpGet("search", Name = "SearchSongs")]
    public ActionResult<ICollection<SongDTO>> SearchSongs([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Ok(new List<SongDTO>());

        var songs = songRepository.Search(q);
        return Ok(songs);
    }
}