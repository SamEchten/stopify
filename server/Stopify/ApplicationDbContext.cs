using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Stopify.Entities.Auth;
using Stopify.Entities.Users;

namespace Stopify;
public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetTableName() == null)
            {
                throw new System.Exception($"{entity} does not contain a table name, please provide one");
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