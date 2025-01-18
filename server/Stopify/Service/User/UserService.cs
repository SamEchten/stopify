using Stopify.Exception.User;
using Stopify.Repository.User;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Service.User;

public class UserService(UserRepository userRepository, HashingService hashingService) : IService
{
    public void AddUser(UserEntity user)
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

    public UserEntity UpdateUser(int id, UserEntity user)
    {
        user.Password = hashingService.HashPassword(user.Password);

        return userRepository.Update(id, user);
    }

    public bool UserExists(int id)
    {
        return userRepository.GetById(id) != null;
    }
}