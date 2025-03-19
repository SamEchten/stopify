namespace Stopify.Services.Streaming;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class WebRTCHub : Hub, IService
{
    public async Task SendOffer(string connectionId, string offer)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveOffer", offer);
    }

    public async Task SendAnswer(string connectionId, string answer)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveAnswer", answer);
    }

    public async Task SendIceCandidate(string connectionId, string candidate)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveIceCandidate", candidate);
    }
}