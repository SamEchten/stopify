namespace Stopify.Services.Session;

public class SessionState
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public int HostUserId { get; set; }
    public int? CurrentSongId { get; set; }
    public double PlaybackPosition { get; set; }
    public bool IsPlaying { get; set; }
    public DateTime? PlaybackStartedAt { get; set; }
    public HashSet<int> MemberUserIds { get; } = [];
    public List<int> Queue { get; } = [];
    public int QueueIndex { get; set; } = -1;

    public double EstimatedPosition =>
        IsPlaying && PlaybackStartedAt.HasValue
            ? PlaybackPosition + (DateTime.UtcNow - PlaybackStartedAt.Value).TotalSeconds
            : PlaybackPosition;
}