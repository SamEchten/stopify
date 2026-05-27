using Microsoft.EntityFrameworkCore;
using Stopify.DTO.Music;
using Stopify.Entities.Music;

namespace Stopify.Repositories.Music;

public class SongRepository(ApplicationDbContext context) : EntityRepository<Song>(context)
{
    public new SongDTO? GetById(int id)
    {
        return DbSet
            .Where(s => s.Id == id)
            .Select(s => new SongDTO
            {
                Id = s.Id,
                Name = s.Name,
                FileLocation = s.FileLocation,
                Artists = s.Artists
                    .Select(a => new ArtistDTO { Id = a.Id, Name = a.Name })
                    .ToList()
            })
            .FirstOrDefault();
    }

    public ICollection<SongDTO> Search(string query)
    {
        return DbSet
            .Include(s => s.Artists)
            .Where(s => s.Name.Contains(query))
            .Take(8)
            .Select(s => new SongDTO
            {
                Id = s.Id,
                Name = s.Name,
                FileLocation = s.FileLocation,
                Artists = s.Artists
                    .Select(a => new ArtistDTO { Id = a.Id, Name = a.Name })
                    .ToList()
            })
            .ToList();
    }
}