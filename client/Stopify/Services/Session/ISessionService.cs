using Stopify.Models.Session;

namespace Stopify.Services.Session;

public interface ISessionService
{
    Task<SessionInfo?> CreateSessionAsync();
    Task<bool> JoinSessionAsync(string sessionId);
    Task<SessionInfo?> GetSessionAsync(string sessionId);
    Task<SessionQueue?> GetQueueAsync(string sessionId);
    Task<bool> AddToQueueAsync(string sessionId, int songId);
    Task<bool> RemoveFromQueueAsync(string sessionId, int index);
    Task<bool> LeaveSessionAsync(string sessionId);
}