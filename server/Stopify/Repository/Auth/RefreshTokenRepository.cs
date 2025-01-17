using Stopify.Entity.Auth;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Repository.Auth;

public class RefreshTokenRepository(ApplicationDbContext context) : EntityRepository<RefreshToken>(context)
{

    public RefreshToken? GetByUser(UserEntity user)
    {
        return DbSet.FirstOrDefault(rt => rt.User == user);
    }

    public RefreshToken? GetByToken(string token)
    {
        return DbSet.FirstOrDefault(rt => rt.Token == token);
    }
    
}