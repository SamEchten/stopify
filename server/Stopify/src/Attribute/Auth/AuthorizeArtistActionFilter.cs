using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Stopify.Repositories.Users;

namespace Stopify.Attribute.Auth;

public class AuthorizeArtistActionFilter(ArtistRepository artistRepository) : AuthorizeActionFilter
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionDescriptor.EndpointMetadata
                .OfType<AuthorizeArtistAttribute>()
                .Any())
        {
            return;
        }

        var request = context.HttpContext.Request;
        var artistIds = new List<int>();

        if (request.HasFormContentType)
        {
            if (request.Form.TryGetValue("ArtistIds", out var artistIdsValues))
            {
                artistIds = artistIdsValues
                    .Select(id => int.TryParse(id, out var parsedId) ? parsedId : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value)
                    .ToList();
            }
        }
        else
        {
            var routeData = context.RouteData.Values;

            if (routeData.TryGetValue("artistId", out var artistRouteId))
            {
                artistIds.Add(Convert.ToInt32(artistRouteId));
            }
        }

        var tokenUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (tokenUserId == null)
        {
            SetContextUnAuthorized(context);
            return;
        }

        var tokenArtist = artistRepository.GetByUserId(Convert.ToInt32(tokenUserId));
        if (tokenArtist == null)
        {
            SetContextUnAuthorized(context);
            return;
        }

        if (artistIds.Contains(tokenArtist.Id)) return;

        SetContextUnAuthorized(context);
    }
}