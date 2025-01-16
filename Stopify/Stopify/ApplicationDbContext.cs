using Microsoft.EntityFrameworkCore;
using Stopify.Entity.User;

namespace Stopify
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}