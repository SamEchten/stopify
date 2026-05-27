namespace Stopify.Services.Session;

public interface ISessionStateService
{
    string? CurrentSessionId { get; }
    bool IsHost { get; }
    event Action? OnChange;
    void SetSession(string id, bool isHost);
    void ClearSession();
}