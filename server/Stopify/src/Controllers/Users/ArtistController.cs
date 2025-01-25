using Microsoft.AspNetCore.Mvc;
using Stopify.Entities.Users;
using Stopify.Repositories.Users;
using Stopify.Requests.Users;
using Stopify.Services.Users;

namespace Stopify.Controllers.Users;

[Controller]
[Route("api/artists")]
public class ArtistController(ArtistService artistService, ArtistRepository artistRepository) : ControllerBase
{
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
}