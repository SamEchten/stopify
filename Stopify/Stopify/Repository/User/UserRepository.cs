using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Repository.User;

public class UserRepository(ApplicationDbContext context) : EntityRepository<UserEntity>(context)
{
    public new void Add(UserEntity user)
    {
        DbSet.Add(user);
        
        Context.SaveChanges();
    }

    public UserEntity? GetByEmail(string email)
    {
        return DbSet.FirstOrDefault(u => u.Email == email);
    }
}
