using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.DTO.Music;
using Stopify.Repositories.Users;
using Stopify.Requests.Music;
using Stopify.Services.Music;

namespace Stopify.Controllers.Music;

[Authorize]
[ApiController]
[Route("api/albums")]
public class AlbumController(AlbumService albumService, ArtistRepository artistRepository) : ControllerBase
{
    [HttpPost(Name = "CreateAlbum")]
    public ActionResult<AlbumDTO> CreateAlbum([FromBody] CreateAlbumRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var artist = artistRepository.GetByUserId(userId);
        if (artist == null) return Forbid();

        var album = albumService.CreateAlbum(request.Title, artist);

        return Ok(new AlbumDTO { Id = album.Id, Title = album.Title, Songs = [] });
    }

    [HttpDelete("{albumId:int}", Name = "DeleteAlbum")]
    public ActionResult DeleteAlbum(int albumId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var artist = artistRepository.GetByUserId(userId);
        if (artist == null) return Forbid();

        var success = albumService.DeleteAlbum(albumId, artist.Id);
        return success ? Ok() : NotFound();
    }

    [HttpPut("{albumId:int}/songs/{songId:int}", Name = "AddSongToAlbum")]
    public ActionResult AddSongToAlbum(int albumId, int songId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var artist = artistRepository.GetByUserId(userId);
        if (artist == null) return Forbid();

        var success = albumService.AddSongToAlbum(albumId, songId, artist.Id);
        return success ? Ok() : NotFound();
    }
}
