using Stopify.Entities.Users;
using Stopify.Enum.Users;
using Stopify.Exceptions.Auth;
using Stopify.Exceptions.Users;
using Stopify.Repositories.Users;
using Stopify.Services.Auth;

namespace Stopify.Services.Users;

public class ArtistService(
    UserService userService,
    ArtistFactory artistFactory,
    ArtistRepository artistRepository,
    UserRepository userRepository,
    HashingService hashingService,
    UserRoleRepository roleRepository): IService
{
    public Artist CreateArtist(string username, string artistName, string email, string password)
    {
        if (artistRepository.GetByName(artistName) != null) throw new ArtistAlreadyExistsException($"Artist with name {artistName} already exists.");

        var user = userRepository.GetByEmail(email);

        if (user == null) {
            user = userService.CreateUser(username, email, password);
        } else {
            if (artistRepository.GetByUser(user) != null) throw new ArtistAlreadyExistsException("An artist account is already linked to this user");
            if (hashingService.HashPassword(password) != user.Password) throw new InvalidLoginCredentialsException();
        }

        var artistRole = roleRepository.getByRole(Role.Artist) ?? throw new Exception("Artist role not found");

        user.AddRole(artistRole);

        var artist = artistFactory.Create(artistName, user);

        artistRepository.Add(artist);

        return artist;
    }
}