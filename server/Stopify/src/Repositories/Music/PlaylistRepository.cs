using Stopify.Entities.Music;
using Stopify.Entities.Users;

namespace Stopify.Repositories.Music;

public class PlaylistRepository(ApplicationDbContext context) : EntityRepository<Playlist>(context)
{
    public ICollection<Playlist> GetByUser(User user)
    {
        return DbSet
            .Where(p => p.User == user)
            .ToList();
    }
}