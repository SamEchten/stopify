using Stopify.Entity.User.DTO;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Repository.User;

public class UserRepository(ApplicationDbContext context) : EntityRepository<UserEntity>(context)
{
    public new IEnumerable<GetAllUsersDTO> GetAll()
    {
        return DbSet.Select(user => new GetAllUsersDTO(user.Id, user.Username)).ToList();
    }
    
    public UserEntity? GetByEmail(string email)
    {
        return DbSet.FirstOrDefault(user => user.Email == email);
    }

    public UserEntity? GetByUsername(string username)
    {
        return DbSet.FirstOrDefault(user => user.Username == username);
    }
}
