using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stopify.Entities.Music;

namespace Stopify.Entities.Users;

[Table("artists")]
public class Artist: Entity
{
    [MaxLength(50)]
    public string Name { get; set; }

    [JsonIgnore]
    public User User { get; init; }

    [JsonIgnore]
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}