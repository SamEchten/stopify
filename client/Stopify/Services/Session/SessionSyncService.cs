using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Stopify.Models.Music;
using Stopify.Models.Session;
using Stopify.Services.Player;

namespace Stopify.Services.Session;

public class SessionSyncService : ISessionSyncService
{
    private readonly HubConnection _connection;
    private readonly HttpClient _httpClient;
    private readonly IPlayerStateService _player;
    private readonly ISessionStateService _sessionState;
    private Dictionary<int, Song> _songCache = [];
    private string? _sessionId;
    private bool _isSynced = true;

    public bool IsConnected => _connection.State == HubConnectionState.Connected;
    public bool IsSynced => _isSynced;

    public event Action? OnStateChanged;
    public event Action<double>? SeekRequested;

    public SessionSyncService(HttpClientHandler httpHandler, CookieContainer cookieContainer, IPlayerStateService player, ISessionStateService sessionState)
    {
        _player = player;
        _sessionState = sessionState;
        _httpClient = new HttpClient(httpHandler, disposeHandler: false)
        {
            BaseAddress = new Uri("http://localhost:8080")
        };
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5232/sessionhub", options =>
            {
                // Give SignalR its own handler so it can dispose it without
                // affecting the shared httpHandler used for REST calls.
                options.HttpMessageHandlerFactory = _ => new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true
                };
            })
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers();
    }

    private void RegisterHandlers()
    {
        _connection.On<int>("OnSongChanged", songId =>
        {
            _isSynced = true;
            if (_songCache.TryGetValue(songId, out var song))
                _player.Play(song);
            OnStateChanged?.Invoke();
        });

        _connection.On<double>("OnPlay", position =>
        {
            if (!_isSynced) return;
            SeekRequested?.Invoke(position);
            _player.Resume();
            OnStateChanged?.Invoke();
        });

        _connection.On<double>("OnPause", position =>
        {
            if (!_isSynced) return;
            SeekRequested?.Invoke(position);
            _player.Pause();
            OnStateChanged?.Invoke();
        });

        _connection.On<double>("OnSeek", position =>
        {
            if (!_isSynced) return;
            SeekRequested?.Invoke(position);
        });

        _connection.On<double>("OnSync", position =>
        {
            if (!_isSynced) return;
            SeekRequested?.Invoke(position);
        });

        _connection.On<List<int>, int>("OnQueueUpdated", (_, _) =>
        {
            OnStateChanged?.Invoke();
        });

        _connection.On("OnQueueEnded", () =>
        {
            _player.Stop();
            OnStateChanged?.Invoke();
        });

        _connection.On("SessionEnded", () =>
        {
            _player.Stop();
            _sessionState.ClearSession();
            OnStateChanged?.Invoke();
        });
    }

    public async Task ConnectAsync(string sessionId)
    {
        _sessionId = sessionId;

        var songs = await _httpClient.GetFromJsonAsync<List<Song>>("/api/songs") ?? [];
        _songCache = songs.ToDictionary(s => s.Id);

        if (_connection.State != HubConnectionState.Disconnected)
            await _connection.StopAsync();

        await _connection.StartAsync();
        await _connection.InvokeAsync("Subscribe", sessionId);
    }

    public async Task DisconnectAsync()
    {
        _sessionId = null;
        if (_connection.State != HubConnectionState.Disconnected)
            await _connection.StopAsync();
    }

    public async Task ChangeSongAsync(int songId)
    {
        if (_sessionId == null || !_sessionState.IsHost) return;
        await _connection.InvokeAsync("ChangeSong", _sessionId, songId);
        await _connection.InvokeAsync("Play", _sessionId, 0.0);
    }

    public async Task PlayAsync(double position)
    {
        if (_sessionId == null || !_sessionState.IsHost) return;
        await _connection.InvokeAsync("Play", _sessionId, position);
    }

    public async Task PauseAsync(double position)
    {
        if (_sessionId == null || !_sessionState.IsHost) return;
        await _connection.InvokeAsync("Pause", _sessionId, position);
    }

    public async Task SeekAsync(double position)
    {
        if (_sessionId == null || !_sessionState.IsHost) return;
        await _connection.InvokeAsync("Seek", _sessionId, position);
    }

    public async Task SongEndedAsync()
    {
        if (_sessionId == null || !_sessionState.IsHost) return;
        await _connection.InvokeAsync("SongEnded", _sessionId);
    }

    public void SetOutOfSync()
    {
        _isSynced = false;
        OnStateChanged?.Invoke();
    }

    public async Task ResyncAsync()
    {
        if (_sessionId == null) return;
        var session = await _httpClient.GetFromJsonAsync<SessionInfo>($"/api/sessions/{Uri.EscapeDataString(_sessionId)}");
        if (session == null) return;

        double position = session.PlaybackPosition;
        if (session.IsPlaying && session.PlaybackStartedAt.HasValue)
            position += (DateTime.UtcNow - session.PlaybackStartedAt.Value.ToUniversalTime()).TotalSeconds;

        SeekRequested?.Invoke(position);

        if (session.IsPlaying)
            _player.Resume();
        else
            _player.Pause();

        _isSynced = true;
        OnStateChanged?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        _httpClient.Dispose();
    }
}