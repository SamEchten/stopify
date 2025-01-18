using Stopify.Entities.Auth;
using Stopify.Entities.Users;

namespace Stopify.Repositories.Auth;

public class RefreshTokenRepository(ApplicationDbContext context) : EntityRepository<RefreshToken>(context)
{
    public RefreshToken? GetByUser(User user)
    {
        return DbSet.FirstOrDefault(rt => rt.User == user);
    }

    public RefreshToken? GetByToken(string token)
    {
        return DbSet.FirstOrDefault(rt => rt.Token == token);
    }
}