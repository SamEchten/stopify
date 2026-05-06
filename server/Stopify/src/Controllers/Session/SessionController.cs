using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stopify.Hubs;
using Stopify.Services.Session;

namespace Stopify.Controllers.Session;

[Authorize]
[ApiController]
[Route("/api/sessions")]
public class SessionController(SessionStore sessionStore, IHubContext<SessionHub> hubContext) : ControllerBase
{
    private int GetUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    public ActionResult GetSessions()
    {
        var sessions = sessionStore.GetAll().Select(s => new
        {
            s.Id,
            s.HostUserId,
            s.CurrentSongId,
            s.IsPlaying,
            MemberCount = s.MemberUserIds.Count
        });

        return Ok(sessions);
    }

    [HttpGet("{sessionId}")]
    public ActionResult GetSession(string sessionId)
    {
        var session = sessionStore.Get(sessionId);

        if (session == null) return NotFound();

        return Ok(new
        {
            session.Id,
            session.HostUserId,
            session.CurrentSongId,
            session.PlaybackPosition,
            session.IsPlaying,
            MemberUserIds = session.MemberUserIds.ToList()
        });
    }

    [HttpPost]
    public ActionResult CreateSession()
    {
        var userId = GetUserId();
        var session = sessionStore.Create(userId);

        return Ok(new
        {
            session.Id,
            session.HostUserId,
            session.CurrentSongId,
            session.PlaybackPosition,
            session.IsPlaying,
            MemberUserIds = session.MemberUserIds.ToList()
        });
    }

    [HttpPost("{sessionId}/join")]
    public async Task<ActionResult> JoinSession(string sessionId)
    {
        var userId = GetUserId();
        var session = sessionStore.Get(sessionId);

        if (session == null) return NotFound();

        session.MemberUserIds.Add(userId);

        await hubContext.Clients.Group(sessionId).SendAsync("UserJoined", userId);

        return Ok(new
        {
            session.Id,
            session.HostUserId,
            session.CurrentSongId,
            session.PlaybackPosition,
            session.IsPlaying,
            MemberUserIds = session.MemberUserIds.ToList()
        });
    }

    [HttpDelete("{sessionId}/leave")]
    public async Task<ActionResult> LeaveSession(string sessionId)
    {
        var userId = GetUserId();
        var session = sessionStore.Get(sessionId);

        if (session == null) return NotFound();

        session.MemberUserIds.Remove(userId);
        await hubContext.Clients.Group(sessionId).SendAsync("UserLeft", userId);

        if (session.HostUserId == userId)
        {
            sessionStore.Remove(sessionId);
            await hubContext.Clients.Group(sessionId).SendAsync("SessionEnded");
        }
        else if (session.MemberUserIds.Count == 0)
        {
            sessionStore.Remove(sessionId);
        }

        return Ok();
    }
}