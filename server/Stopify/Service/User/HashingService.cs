using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Stopify.Service.User;

public class HashingService(IOptions<AppSettings> appSettings): IService
{
    public string HashPassword(string password)
    {
        var salt = appSettings.Value.Salt;
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = SHA256.HashData(combined);
        return Convert.ToBase64String(hash);
    }
}
