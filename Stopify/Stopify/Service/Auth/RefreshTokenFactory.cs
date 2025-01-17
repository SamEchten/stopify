using Stopify.Entity.Auth;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Service.Auth;

public class RefreshTokenFactory: IFactory
{

    public RefreshToken Build(string token, UserEntity user)
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
