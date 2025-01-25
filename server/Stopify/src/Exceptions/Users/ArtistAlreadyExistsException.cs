using System.Net;

namespace Stopify.Exceptions.Users;

public class ArtistAlreadyExistsException(string message) : HttpException(message, HttpStatusCode.BadRequest);