﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Stopify.Middleware.Exception;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        
        logger.LogError(exception, "An unhandled exception occurred.");

        if (exception is HttpRequestException)
        {
            context.Result = new ObjectResult(new
            {
                message = exception.Message
            })
            {
                StatusCode = 400
            };
                
            return;
        }

        context.Result = new ObjectResult(new
        {
            message = "Oops! Something went wrong."
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}


