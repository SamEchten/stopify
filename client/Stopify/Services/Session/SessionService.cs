using System.Net.Http.Json;
using Stopify.Models.Session;

namespace Stopify.Services.Session;

public class SessionService(HttpClient http) : ISessionService
{
    public async Task<SessionInfo?> CreateSessionAsync()
    {
        var response = await http.PostAsync("/api/sessions", null);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<SessionInfo>();
    }

    public async Task<bool> JoinSessionAsync(string sessionId)
    {
        var response = await http.PostAsync($"/api/sessions/{Uri.EscapeDataString(sessionId)}/join", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<SessionInfo?> GetSessionAsync(string sessionId)
    {
        try { return await http.GetFromJsonAsync<SessionInfo>($"/api/sessions/{Uri.EscapeDataString(sessionId)}"); }
        catch { return null; }
    }

    public async Task<SessionQueue?> GetQueueAsync(string sessionId)
    {
        try { return await http.GetFromJsonAsync<SessionQueue>($"/api/sessions/{Uri.EscapeDataString(sessionId)}/queue"); }
        catch { return null; }
    }

    public async Task<bool> AddToQueueAsync(string sessionId, int songId)
    {
        var response = await http.PostAsJsonAsync($"/api/sessions/{Uri.EscapeDataString(sessionId)}/queue", new { SongId = songId });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveFromQueueAsync(string sessionId, int index)
    {
        var response = await http.DeleteAsync($"/api/sessions/{Uri.EscapeDataString(sessionId)}/queue/{index}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LeaveSessionAsync(string sessionId)
    {
        var response = await http.DeleteAsync($"/api/sessions/{Uri.EscapeDataString(sessionId)}/leave");
        return response.IsSuccessStatusCode;
    }
}