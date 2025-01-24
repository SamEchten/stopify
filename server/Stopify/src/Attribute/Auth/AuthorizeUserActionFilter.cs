using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Stopify.Attribute.Auth;

public class AuthorizeUserActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionDescriptor.EndpointMetadata
                .OfType<AuthorizeUserAttribute>()
                .Any())
        {
            return;
        }

        var routeData = context.RouteData.Values;

        if (!routeData.TryGetValue("userId", out var value)) return;

        var routeUserId = value?.ToString();
        var tokenUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (routeUserId == null || tokenUserId == null || routeUserId == tokenUserId) return;

        context.HttpContext.Response.StatusCode = 403;
        context.HttpContext.Response.ContentType = "application/json";

        context.HttpContext.Response.WriteAsJsonAsync(new {message = "Forbidden: You are not authorized to perform this action"});
        context.Result = new EmptyResult();
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}