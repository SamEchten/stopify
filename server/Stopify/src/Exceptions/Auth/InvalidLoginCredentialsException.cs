using System.Net;

namespace Stopify.Exceptions.Auth;

public class InvalidLoginCredentialsException() : HttpException("Invalid login credentials provided", HttpStatusCode.BadRequest);