namespace Stopify.Models.Session;

public class SessionQueue
{
    public List<int> Queue { get; set; } = [];
    public int QueueIndex { get; set; }
}