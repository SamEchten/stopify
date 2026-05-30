using Stopify.Models.Music;

namespace Stopify.Services.Player;

public class PlayerStateService : IPlayerStateService
{
    private readonly List<Song> _queue = [];
    private readonly List<Song> _history = [];

    public Song? CurrentSong { get; private set; }
    public bool IsPlaying { get; private set; }
    public IReadOnlyList<Song> Queue => _queue;
    public int QueueIndex { get; private set; } = -1;
    public int PlayVersion { get; private set; }
    public bool HasNext => _queue.Count > 0;
    public bool HasPrevious => _history.Count > 0;

    public event Action? OnChange;
    public event Action? OnQueueChange;

    public void Play(Song song)
    {
        _history.Clear();
        _queue.Clear();
        CurrentSong = song;
        IsPlaying = true;
        QueueIndex = -1;
        PlayVersion++;
        OnQueueChange?.Invoke();
        OnChange?.Invoke();
    }

    public void PlayWithContext(Song song, IEnumerable<Song> context)
    {
        _history.Clear();
        _queue.Clear();
        CurrentSong = song;
        IsPlaying = true;
        QueueIndex = -1;
        PlayVersion++;
        foreach (var s in context.SkipWhile(s => s.Id != song.Id).Skip(1))
            _queue.Add(s);
        OnQueueChange?.Invoke();
        OnChange?.Invoke();
    }

    public void Pause()
    {
        IsPlaying = false;
        OnChange?.Invoke();
    }

    public void Resume()
    {
        if (CurrentSong == null) return;
        IsPlaying = true;
        OnChange?.Invoke();
    }

    public void Stop()
    {
        CurrentSong = null;
        IsPlaying = false;
        QueueIndex = -1;
        OnChange?.Invoke();
    }

    public void AddToQueue(Song song)
    {
        _queue.Add(song);
        if (CurrentSong == null)
        {
            CurrentSong = _queue[0];
            _queue.RemoveAt(0);
            IsPlaying = true;
            PlayVersion++;
            OnChange?.Invoke();
        }
        OnQueueChange?.Invoke();
    }

    public void RemoveFromQueue(int index)
    {
        if (index < 0 || index >= _queue.Count) return;
        _queue.RemoveAt(index);
        OnQueueChange?.Invoke();
    }

    public void SkipNext()
    {
        if (_queue.Count > 0)
        {
            if (CurrentSong != null)
                _history.Add(CurrentSong);
            CurrentSong = _queue[0];
            _queue.RemoveAt(0);
            IsPlaying = true;
            PlayVersion++;
            OnQueueChange?.Invoke();
        }
        else
        {
            CurrentSong = null;
            IsPlaying = false;
            QueueIndex = -1;
        }
        OnChange?.Invoke();
    }

    public void SkipPrevious()
    {
        if (_history.Count == 0) return;
        if (CurrentSong != null)
            _queue.Insert(0, CurrentSong);
        CurrentSong = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        IsPlaying = true;
        PlayVersion++;
        OnQueueChange?.Invoke();
        OnChange?.Invoke();
    }
}