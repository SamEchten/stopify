using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Stopify.Enum.Users;

namespace Stopify.Entities.Users;

[Index(nameof(Email), IsUnique = true)]
public class User: Entity
{
    [MaxLength(20)]
    public string Username { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    public ICollection<UserRole> Roles { get; init; } = new List<UserRole>();

    [MaxLength(255)]
    [JsonIgnore]
    public string Password { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public DateTime UpdatedAt { get; set; }

    public void AddRole(UserRole role)
    {
        if (Roles.All(r => r.Name != role.Name)) // Check if role with same name doesn't exist
        {
            Roles.Add(role);
        }
    }
}
