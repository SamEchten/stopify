using Microsoft.EntityFrameworkCore;
using Stopify.DTO.Music;
using Stopify.Entities.Music;

namespace Stopify.Repositories.Music;

public class AlbumRepository(ApplicationDbContext context) : EntityRepository<Album>(context)
{
    public ICollection<AlbumDTO> GetByArtistId(int artistId)
    {
        return DbSet
            .Where(a => a.Artist.Id == artistId)
            .Include(a => a.Songs)
            .Select(a => new AlbumDTO
            {
                Id = a.Id,
                Title = a.Title,
                Songs = a.Songs.Select(s => new SongDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    FileLocation = s.FileLocation,
                    Duration = s.Duration,
                    Artists = s.Artists.Select(ar => new ArtistDTO { Id = ar.Id, Name = ar.Name }).ToList()
                }).ToList()
            })
            .ToList();
    }
}