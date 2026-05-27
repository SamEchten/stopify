using Stopify.Models.Music;

namespace Stopify.Services.Player;

public interface IPlayerStateService
{
    Song? CurrentSong { get; }
    bool IsPlaying { get; }
    IReadOnlyList<Song> Queue { get; }
    int QueueIndex { get; }
    int PlayVersion { get; }
    bool HasNext { get; }
    bool HasPrevious { get; }
    event Action? OnChange;
    event Action? OnQueueChange;
    void Play(Song song);
    void Pause();
    void Resume();
    void Stop();
    void AddToQueue(Song song);
    void RemoveFromQueue(int index);
    void SkipNext();
    void SkipPrevious();
}
