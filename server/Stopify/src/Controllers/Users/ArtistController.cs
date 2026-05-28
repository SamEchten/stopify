using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stopify.DTO.Music;
using Stopify.Entities.Users;
using Stopify.Repositories.Music;
using Stopify.Repositories.Users;
using Stopify.Requests.Users;
using Stopify.Services.Users;

namespace Stopify.Controllers.Users;

[Controller]
[Route("api/artists")]
public class ArtistController(ArtistService artistService, ArtistRepository artistRepository, SongRepository songRepository, AlbumRepository albumRepository) : ControllerBase
{
    [Authorize]
    [HttpGet("me", Name = "GetMyArtist")]
    public ActionResult<Artist> GetMe()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var artist = artistRepository.GetByUserId(userId);
        return artist == null ? NotFound() : Ok(artist);
    }

    [HttpPost(Name = "CreateArtist")]
    public ActionResult<Artist> CreateArtist(CreateArtistRequest request)
    {
        var artist = artistService.CreateArtist(request.Username, request.ArtistName, request.Email, request.Password);

        return artist;
    }

    [HttpGet("{artistId:int}", Name = "GetArtist")]
    public ActionResult<Artist> Get(int artistId)
    {
        var artist = artistRepository.GetById(artistId);

        return artist == null ? NotFound() : Ok(artist);
    }

    [HttpGet(Name = "GetAllArtists")]
    public ActionResult<ICollection<Artist>> GetAll()
    {
        var artists = artistRepository.GetAll();

        return Ok(artists);
    }

    [HttpGet("{artistId:int}/songs", Name = "GetArtistSongs")]
    public ActionResult<ICollection<SongDTO>> GetSongs(int artistId)
    {
        var artist = artistRepository.GetById(artistId);
        if (artist == null) return NotFound();

        return Ok(songRepository.GetByArtistId(artistId));
    }

    [HttpGet("{artistId:int}/albums", Name = "GetArtistAlbums")]
    public ActionResult<ICollection<AlbumDTO>> GetAlbums(int artistId)
    {
        var artist = artistRepository.GetById(artistId);
        if (artist == null) return NotFound();

        return Ok(albumRepository.GetByArtistId(artistId));
    }
}