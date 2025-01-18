using Microsoft.EntityFrameworkCore;
using Stopify.Entity.Auth;
using Stopify.Entity.User;

namespace Stopify;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
}