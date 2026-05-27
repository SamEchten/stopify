using Microsoft.EntityFrameworkCore;
using Stopify.Entities.Music;
using Stopify.Entities.Users;

namespace Stopify.Repositories.Music;

public class PlaylistRepository(ApplicationDbContext context) : EntityRepository<Playlist>(context)
{
    public new Playlist? GetById(int id)
    {
        return DbSet
            .Include(p => p.Songs)
                .ThenInclude(s => s.Artists)
            .SingleOrDefault(p => p.Id == id);
    }

    public ICollection<Playlist> GetByUser(User user)
    {
        return DbSet
            .Where(p => p.User == user)
            .ToList();
    }
}