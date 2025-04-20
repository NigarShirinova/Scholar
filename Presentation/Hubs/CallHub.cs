using Microsoft.AspNetCore.SignalR;

namespace Presentation.Hubs
{
    public class CallHub : Hub
    {
        public async Task SendOffer(string toUserId, string offer)
            => await Clients.User(toUserId).SendAsync("ReceiveOffer", Context.UserIdentifier, offer);

        public async Task SendAnswer(string toUserId, string answer)
            => await Clients.User(toUserId).SendAsync("ReceiveAnswer", Context.UserIdentifier, answer);

        public async Task SendCandidate(string toUserId, string candidate)
            => await Clients.User(toUserId).SendAsync("ReceiveCandidate", Context.UserIdentifier, candidate);
    }
}
