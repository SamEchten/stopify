using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Stopify.Attribute.Auth;

public abstract class AuthorizeActionFilter : IActionFilter
{
    public abstract void OnActionExecuting(ActionExecutingContext context);

    public void OnActionExecuted(ActionExecutedContext context) {}

    protected static void SetContextUnAuthorized(ActionExecutingContext context)
    {
        context.HttpContext.Response.StatusCode = 403;
        context.HttpContext.Response.ContentType = "application/json";

        context.HttpContext.Response.WriteAsJsonAsync(new {message = "Forbidden: You are not authorized to perform this action"});
        context.Result = new EmptyResult();
    }
}