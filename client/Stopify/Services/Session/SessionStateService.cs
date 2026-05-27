namespace Stopify.Services.Session;

public class SessionStateService : ISessionStateService
{
    public string? CurrentSessionId { get; private set; }
    public bool IsHost { get; private set; }
    public event Action? OnChange;

    public void SetSession(string id, bool isHost)
    {
        CurrentSessionId = id;
        IsHost = isHost;
        OnChange?.Invoke();
    }

    public void ClearSession()
    {
        CurrentSessionId = null;
        IsHost = false;
        OnChange?.Invoke();
    }
}