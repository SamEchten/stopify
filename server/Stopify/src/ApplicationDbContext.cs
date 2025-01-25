using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Stopify.Entities.Auth;
using Stopify.Entities.Users;
using Stopify.Enum.Users;
using Stopify.Services.Users;

namespace Stopify;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<UserRole> UserRoles { get; set; }
    public required DbSet<Artist> Artists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, Name = nameof(Role.User) },
            new UserRole { Id = 2, Name = nameof(Role.Artist) }
        );

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetTableName() == null)
            {
                throw new Exception($"{entity} does not contain a table name, please provide one");
            }

            entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }
    }

    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        var regex = MyRegex();
        return regex.Replace(name, "$1_$2").ToLower();
    }

    [GeneratedRegex("([a-z0-9])([A-Z])")]
    private static partial Regex MyRegex();
}