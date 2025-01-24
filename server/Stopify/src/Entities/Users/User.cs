using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Stopify.Entities.Users;

[Index(nameof(Email), IsUnique = true)]
public class User: Entity
{
    [MaxLength(20)]
    public required string Username { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }

    public required ICollection<UserRole> Roles { get; init; }

    [MaxLength(255)]
    public required string Password { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; init; }
}
