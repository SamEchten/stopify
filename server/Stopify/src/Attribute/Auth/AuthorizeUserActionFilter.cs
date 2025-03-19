using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Stopify.Attribute.Auth;

public class AuthorizeUserActionFilter : AuthorizeActionFilter
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionDescriptor.EndpointMetadata
                .OfType<AuthorizeUserAttribute>()
                .Any())
        {
            return;
        }

        var routeData = context.RouteData.Values;

        if (!routeData.TryGetValue("userId", out var value))
        {
            SetContextUnAuthorized(context);
            return;
        }

        var routeUserId = value?.ToString();
        var tokenUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (routeUserId == null || tokenUserId == null || routeUserId == tokenUserId) return;

        SetContextUnAuthorized(context);
    }
}