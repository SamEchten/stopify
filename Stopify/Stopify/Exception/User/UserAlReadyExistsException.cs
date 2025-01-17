using System.Net;

namespace Stopify.Exception.User;
public class UserAlreadyExistsException() : HttpException("User already exists.", HttpStatusCode.BadRequest);
