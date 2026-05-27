namespace Stopify.Services.Session;

public interface ISessionSyncService : IAsyncDisposable
{
    bool IsConnected { get; }
    bool IsSynced { get; }

    event Action? OnStateChanged;
    event Action<double>? SeekRequested;

    Task ConnectAsync(string sessionId);
    Task DisconnectAsync();

    Task ChangeSongAsync(int songId);
    Task PlayAsync(double position);
    Task PauseAsync(double position);
    Task SeekAsync(double position);
    Task SongEndedAsync();
    void SetOutOfSync();
    Task ResyncAsync();
}