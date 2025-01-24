using Stopify.Entities.Users;
using Stopify.Entities.Users.DTO;
using Stopify.Enum.Users;

namespace Stopify.Repositories.Users;

public class UserRoleRepository(ApplicationDbContext context) : EntityRepository<UserRole>(context)
{
    public UserRole? getByRole(Role role)
    {
        return context.UserRoles.FirstOrDefault(e => e.Name == role.ToString());
    }
}