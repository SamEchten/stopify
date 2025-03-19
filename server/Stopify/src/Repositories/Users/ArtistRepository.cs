using Stopify.Entities.Users;

namespace Stopify.Repositories.Users;

public class ArtistRepository(ApplicationDbContext context) : EntityRepository<Artist>(context)
{
    public Artist? GetByName(string artistName)
    {
        return DbSet
            .FirstOrDefault(a => a.Name == artistName);
    }

    public Artist? GetByUser(User user)
    {
        return DbSet
            .FirstOrDefault(a => a.User.Id == user.Id);
    }

    public Artist? GetByUserId(int userId)
    {
        return DbSet
            .FirstOrDefault(a => a.User.Id == userId);
    }
}