using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Stopify.Entities.Users;

public class UserRole : Entity
{
    [MaxLength(20)]
    public required string Name { get; init; }

    [JsonIgnore]
    public ICollection<User> Users { get; init; }
}