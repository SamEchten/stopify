using System.Net;

namespace Stopify.Exceptions.Auth;
public class UnauthorizedRequestException() : HttpException("You cannot perform this action", HttpStatusCode.Forbidden);