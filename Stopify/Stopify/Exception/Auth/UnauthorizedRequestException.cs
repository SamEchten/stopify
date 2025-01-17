using System.Net;

namespace Stopify.Exception.Auth;
public class UnauthorizedRequestException() : HttpException("You cannot perform this action", HttpStatusCode.Forbidden);