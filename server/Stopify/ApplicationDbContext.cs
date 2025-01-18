using Microsoft.EntityFrameworkCore;
using Stopify.Entity.Auth;
using Stopify.Entity.User;

namespace Stopify;
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}