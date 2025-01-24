using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Stopify.Entities.Users;

namespace Stopify.Entities.Auth;

public class RefreshToken: Entity
{
    [MaxLength(255)]
    public required string Token { get; init; }

    [ForeignKey("user_id")]
    public required User User { get; init; }

    public required DateTime ExpiresAt { get; init; }

    public required DateTime CreatedAt { get; init; }
}