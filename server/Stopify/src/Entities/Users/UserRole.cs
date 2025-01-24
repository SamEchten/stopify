using System.ComponentModel.DataAnnotations;

namespace Stopify.Entities.Users;

public class UserRole : Entity
{
    [MaxLength(20)]
    public required string Name { get; init; }

    public ICollection<User> Users { get; init; }
}