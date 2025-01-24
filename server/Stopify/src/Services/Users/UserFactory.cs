using Stopify.Entities.Users;
using Stopify.Enum.Users;
using Stopify.Repositories.Users;
using Stopify.Services.Auth;

namespace Stopify.Services.Users;

public class UserFactory(HashingService hashingService, UserRoleRepository roleRepository) : IFactory
{
    public User Create(string username, string email, string password, Role role = Role.User)
    {
        var userRole = roleRepository.getByRole(role) ?? throw new Exception("Provided role not found");

        return new User
        {
            Username = username,
            Email = email,
            Roles = [ userRole ],
            Password = hashingService.HashPassword(password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
}