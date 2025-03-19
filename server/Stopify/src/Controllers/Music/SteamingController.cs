using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.Attribute.Auth;
using Stopify.Entities.Music;
using Stopify.Repositories.Music;
using Stopify.Requests.Music;
using Stopify.Services.Music;

namespace Stopify.Controllers.Music;

// [Authorize]
[ApiController]
[Route("/api/stream")]
public class StreamingController(SongRepository songRepository) : ControllerBase
{
    [HttpGet("{songId:int}")]
    public IActionResult StreamAudio(int songId)
    {
        var song = songRepository.GetById(songId);
        if (song == null)
        {
            return NotFound();
        }

        var stream = System.IO.File.OpenRead(song.FileLocation);
        return File(stream, "audio/mpeg", enableRangeProcessing: true);
    }
}