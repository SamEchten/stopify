using Stopify.Entities.Users;

namespace Stopify.Services.Users;

public class ArtistFactory : IFactory
{
    public Artist Create(string artistName, User user)
    {
        return new Artist
        {
            Name = artistName,
            User = user
        };
    }
}