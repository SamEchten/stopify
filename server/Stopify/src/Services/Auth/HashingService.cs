using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Stopify.Services.Auth;

public class HashingService(IOptions<AppSettings> appSettings): IService
{
    public string HashPassword(string password)
    {
        var salt = appSettings.Value.Salt;
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = SHA256.HashData(combined);
        return Convert.ToBase64String(hash);
    }

    public string GenerateUniqueFileNameHash(IFormFile file)
    {
        using var sha256 = SHA256.Create();
        using var stream = file.OpenReadStream();

        var fileHash = sha256.ComputeHash(stream);

        var salt = Guid.NewGuid().ToString();

        var combined = BitConverter.ToString(fileHash).Replace("-", "").ToLower() + salt;

        var finalHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));

        return BitConverter.ToString(finalHash).Replace("-", "").ToLower() + Path.GetExtension(file.FileName);
    }
}
