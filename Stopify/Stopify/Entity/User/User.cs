using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Stopify.Entity.User;
    
[Index(nameof(Email), IsUnique = true)]
public class User: Entity
{
    [MaxLength(20)]
    public required string Username { get; set; }
    
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    
    [MaxLength(255)]
    public required string Password { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}
