using System.Collections.Concurrent;

namespace Stopify.Services.Session;

public class SessionStore
{
    private readonly ConcurrentDictionary<string, SessionState> _sessions = new();

    public SessionState Create(int hostUserId)
    {
        var session = new SessionState { HostUserId = hostUserId };
        session.MemberUserIds.Add(hostUserId);
        _sessions[session.Id] = session;
        return session;
    }

    public SessionState? Get(string sessionId) => _sessions.GetValueOrDefault(sessionId);

    public IEnumerable<SessionState> GetAll() => _sessions.Values;

    public bool Remove(string sessionId) => _sessions.TryRemove(sessionId, out _);
}