using System.Net;

namespace Stopify.Exceptions.Users;
public class UserAlreadyExistsException() : HttpException("User already exists.", HttpStatusCode.BadRequest);
