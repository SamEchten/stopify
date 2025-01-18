using Stopify.Exceptions.Users;
using Stopify.Repositories.Users;
using Stopify.Entities.Users;

namespace Stopify.Services.Users;

public class UserService(UserRepository userRepository, HashingService hashingService) : IService
{
    public void AddUser(User user)
    {
        if (userRepository.GetByEmail(user.Email) != null)
        {
            throw new UserAlreadyExistsException();
        }

        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;

        user.Password = hashingService.HashPassword(user.Password);

        userRepository.Add(user);
    }

    public User UpdateUser(int id, User user)
    {
        user.Password = hashingService.HashPassword(user.Password);

        return userRepository.Update(id, user);
    }

    public bool UserExists(int id)
    {
        return userRepository.GetById(id) != null;
    }
}