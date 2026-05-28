using Stopify.Entities.Music;
using Stopify.Entities.Users;

namespace Stopify.Services.Music;

public class AlbumFactory : IFactory
{
    public Album Create(string title, Artist artist)
    {
        return new Album
        {
            Title = title,
            Artist = artist
        };
    }
}
