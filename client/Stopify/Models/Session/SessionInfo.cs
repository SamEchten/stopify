namespace Stopify.Models.Session;

public class SessionInfo
{
    public string Id { get; set; } = string.Empty;
    public int HostUserId { get; set; }
    public int? CurrentSongId { get; set; }
    public double PlaybackPosition { get; set; }
    public bool IsPlaying { get; set; }
    public DateTime? PlaybackStartedAt { get; set; }
    public List<int> MemberUserIds { get; set; } = [];
}