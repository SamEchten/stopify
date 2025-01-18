using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Entity.Auth;

public class RefreshToken: Entity
{
    [MaxLength(255)]
    public required string Token { get; init; }

    [ForeignKey("UserId")]
    public required UserEntity User { get; set; }

    public required DateTime ExpiresAt { get; init; }

    public required DateTime CreatedAt { get; init; }
}