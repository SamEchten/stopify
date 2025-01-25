using Stopify.Exceptions.Users;
using Stopify.Repositories.Users;
using Stopify.Entities.Users;

namespace Stopify.Services.Users;

public class UserService(UserRepository userRepository, UserFactory userFactory) : IService
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
}