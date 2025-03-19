using Stopify.Entities.Music;
using Stopify.Entities.Users;
using Stopify.Repositories.Users;

namespace Stopify.Services.Music;

public class PlaylistFactory(UserRepository userRepository) : IFactory
{
    public Playlist CreatePlaylist(User user, string title)
    {
        return new Playlist
        {
            Title = title,
            User = user,
        };
    }
}