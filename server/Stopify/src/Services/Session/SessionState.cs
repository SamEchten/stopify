namespace Stopify.Services.Session;

public class SessionState
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public int HostUserId { get; set; }
    public int? CurrentSongId { get; set; }
    public double PlaybackPosition { get; set; }
    public bool IsPlaying { get; set; }
    public HashSet<int> MemberUserIds { get; } = [];
}