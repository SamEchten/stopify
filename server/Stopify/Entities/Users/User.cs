using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Stopify.Entities.Users;

[Index(nameof(Email), IsUnique = true)]
public class User: Entity
{
    [MaxLength(20)]
    public required string Username { get; init; }

    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; init; }

    [MaxLength(255)]
    public required string Password { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required DateTime UpdatedAt { get; set; }
}
