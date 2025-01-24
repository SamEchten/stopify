using Stopify.Entities.Auth;
using Stopify.Entities.Users;

namespace Stopify.Services.Auth;

public class RefreshTokenFactory: IFactory
{
    public static RefreshToken Build(string token, User user)
    {
        return new RefreshToken
        {
            Token = token,
            User = user,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(7),
        };
    }
}
