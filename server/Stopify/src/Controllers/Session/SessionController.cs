using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stopify.Hubs;
using Stopify.Requests.Session;
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
            session.PlaybackStartedAt,
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

    [HttpGet("{sessionId}/queue")]
    public ActionResult GetQueue(string sessionId)
    {
        var session = sessionStore.Get(sessionId);
        if (session == null) return NotFound();

        return Ok(new { session.Queue, session.QueueIndex });
    }

    [HttpPost("{sessionId}/queue")]
    public async Task<ActionResult> AddToQueue(string sessionId, [FromBody] AddToQueueRequest request)
    {
        var session = sessionStore.Get(sessionId);
        if (session == null) return NotFound();

        session.Queue.Add(request.SongId);

        await hubContext.Clients.Group(sessionId).SendAsync("OnQueueUpdated", session.Queue, session.QueueIndex);

        return Ok(new { session.Queue, session.QueueIndex });
    }

    [HttpPost("{sessionId}/next")]
    public async Task<ActionResult> SkipNext(string sessionId)
    {
        var session = sessionStore.Get(sessionId);
        if (session == null) return NotFound();
        var userId = GetUserId();
        if (session.HostUserId != userId)
            return StatusCode(403, new { message = $"Only the host can do this (host: {session.HostUserId}, you: {userId})" });

        var nextIndex = session.QueueIndex + 1;
        if (nextIndex >= session.Queue.Count)
            return BadRequest(new { message = "No next song in queue" });

        session.QueueIndex = nextIndex;
        var songId = session.Queue[nextIndex];
        session.CurrentSongId = songId;
        session.PlaybackPosition = 0;
        session.IsPlaying = true;
        session.PlaybackStartedAt = DateTime.UtcNow;

        await hubContext.Clients.Group(sessionId).SendAsync("OnSongChanged", songId);
        await hubContext.Clients.Group(sessionId).SendAsync("OnPlay", 0.0, CancellationToken.None);

        return Ok(new { session.Queue, session.QueueIndex });
    }

    [HttpPost("{sessionId}/previous")]
    public async Task<ActionResult> SkipPrevious(string sessionId)
    {
        var session = sessionStore.Get(sessionId);
        if (session == null) return NotFound();
        var userId = GetUserId();
        if (session.HostUserId != userId)
            return StatusCode(403, new { message = $"Only the host can do this (host: {session.HostUserId}, you: {userId})" });

        var prevIndex = session.QueueIndex - 1;
        if (prevIndex < 0)
            return BadRequest(new { message = "No previous song in queue" });

        session.QueueIndex = prevIndex;
        var songId = session.Queue[prevIndex];
        session.CurrentSongId = songId;
        session.PlaybackPosition = 0;
        session.IsPlaying = true;
        session.PlaybackStartedAt = DateTime.UtcNow;

        await hubContext.Clients.Group(sessionId).SendAsync("OnSongChanged", songId);
        await hubContext.Clients.Group(sessionId).SendAsync("OnPlay", 0.0, CancellationToken.None);

        return Ok(new { session.Queue, session.QueueIndex });
    }
}