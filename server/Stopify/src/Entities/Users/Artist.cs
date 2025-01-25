using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Stopify.Entities.Users;

[Table("artists")]
public class Artist: Entity
{
    public string Name { get; set; }

    [JsonIgnore]
    public User User { get; set; }
}