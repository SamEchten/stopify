using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Stopify.Services.Session;

namespace Stopify.Hubs;

[Authorize]
public class SessionHub(SessionStore sessionStore) : Hub
{
    private int GetUserId() =>
        int.Parse(Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public async Task Subscribe(string sessionId)
    {
        var session = sessionStore.Get(sessionId);

        if (session == null)
        {
            await Clients.Caller.SendAsync("Error", "Session not found");
            return;
        }

        if (!session.MemberUserIds.Contains(GetUserId()))
        {
            await Clients.Caller.SendAsync("Error", "You are not a member of this session");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
    }

    public async Task Play(string sessionId, double position)
    {
        if (!IsHost(sessionId)) return;

        var session = sessionStore.Get(sessionId)!;
        session.IsPlaying = true;
        session.PlaybackPosition = position;

        await Clients.Group(sessionId).SendAsync("OnPlay", position);
    }

    public async Task Pause(string sessionId, double position)
    {
        if (!IsHost(sessionId)) return;

        var session = sessionStore.Get(sessionId)!;
        session.IsPlaying = false;
        session.PlaybackPosition = position;

        await Clients.Group(sessionId).SendAsync("OnPause", position);
    }

    public async Task Seek(string sessionId, double position)
    {
        if (!IsHost(sessionId)) return;

        var session = sessionStore.Get(sessionId)!;
        session.PlaybackPosition = position;

        await Clients.Group(sessionId).SendAsync("OnSeek", position);
    }

    public async Task ChangeSong(string sessionId, int songId)
    {
        if (!IsHost(sessionId)) return;

        var session = sessionStore.Get(sessionId)!;
        session.CurrentSongId = songId;
        session.PlaybackPosition = 0;
        session.IsPlaying = false;

        await Clients.Group(sessionId).SendAsync("OnSongChanged", songId);
    }

    public async Task Sync(string sessionId, double position)
    {
        if (!IsHost(sessionId)) return;

        var session = sessionStore.Get(sessionId)!;
        session.PlaybackPosition = position;

        await Clients.OthersInGroup(sessionId).SendAsync("OnSync", position);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();

        var hostedSessions = sessionStore.GetAll()
            .Where(s => s.HostUserId == userId)
            .ToList();

        foreach (var session in hostedSessions)
        {
            sessionStore.Remove(session.Id);
            await Clients.Group(session.Id).SendAsync("SessionEnded");
        }

        await base.OnDisconnectedAsync(exception);
    }

    private bool IsHost(string sessionId)
    {
        var session = sessionStore.Get(sessionId);
        return session != null && session.HostUserId == GetUserId();
    }

    private static object ToDto(SessionState session) => new
    {
        session.Id,
        session.HostUserId,
        session.CurrentSongId,
        session.PlaybackPosition,
        session.IsPlaying,
        MemberUserIds = session.MemberUserIds.ToList()
    };
}