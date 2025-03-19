using Stopify.Entities.Music;
using Stopify.Entities.Users;

namespace Stopify.Services.Music;

public class SongFactory: IFactory
{
    public Song Create(string songName, ICollection<Artist> artists, string fileLocation)
    {
        return new Song
        {
            Name = songName,
            Artists = artists,
            FileLocation = fileLocation,
        };
    }
}