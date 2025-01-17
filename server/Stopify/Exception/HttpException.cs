﻿using System.Net;

namespace Stopify.Exception;

public abstract class HttpException(string message, HttpStatusCode statusCode) : System.Exception(message)
{
    public int StatusCode { get; } = (int) statusCode;
}