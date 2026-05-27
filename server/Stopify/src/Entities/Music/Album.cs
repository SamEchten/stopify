using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stopify.Entities.Users;

namespace Stopify.Entities.Music;

[Table("albums")]
public class Album : Entity
{
    [MaxLength(100)]
    public string Title { get; set; }

    public ICollection<Song> Songs { get; set; } = new List<Song>();

    [JsonIgnore]
    public Artist Artist { get; set; }
}