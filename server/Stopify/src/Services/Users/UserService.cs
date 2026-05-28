using Stopify.Exceptions.Users;
using Stopify.Repositories.Users;
using Stopify.Entities.Users;
using Stopify.Services.Auth;

namespace Stopify.Services.Users;

public class UserService(UserRepository userRepository, UserFactory userFactory, HashingService hashingService) : IService
{
    public User CreateUser(string username, string email, string password)
    {
        if (userRepository.GetByEmail(email) != null)
        {
            throw new UserAlreadyExistsException();
        }

        var user = userFactory.Create(username, email, password);

        userRepository.Add(user);

        return user;
    }

    public User UpdateUser(int id, string? username, string? email)
    {
        var user = userRepository.GetById(id)!;

        if (username != null) user.Username = username;
        if (email != null) user.Email = email;

        return userRepository.Update(user);
    }

    public bool UserExists(int id)
    {
        return userRepository.GetById(id) != null;
    }

    public bool ChangePassword(int userId, string currentPassword, string newPassword)
    {
        var user = userRepository.GetById(userId);
        if (user == null) return false;

        if (user.Password != hashingService.HashPassword(currentPassword)) return false;

        user.Password = hashingService.HashPassword(newPassword);
        userRepository.Save();
        return true;
    }
}