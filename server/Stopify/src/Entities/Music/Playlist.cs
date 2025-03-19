using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stopify.Entities.Users;

namespace Stopify.Entities.Music;

[Table("playlists")]
public class Playlist : Entity
{
    [MaxLength(50)]
    public string Title { get; set; }

    public ICollection<Song> Songs { get; set; } = new List<Song>();

    [JsonIgnore]
    public User User { get; set; }

    public void AddSong(Song song)
    {
        Songs.Add(song);
    }

    public void DeleteSong(Song song)
    {
        if (Songs.Contains(song))
        {
            Songs.Remove(song);
        }
    }
}