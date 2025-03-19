using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stopify.Entities.Users;

namespace Stopify.Entities.Music;

[Table("songs")]
public class Song : Entity
{
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string FileLocation { get; set; }

    [JsonIgnore]
    public ICollection<Artist> Artists { get; set; } = new List<Artist>();

    [JsonIgnore]
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}