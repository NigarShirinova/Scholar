using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CallHub : Hub
{
    private static readonly Dictionary<string, List<string>> lessonGroups = new();

    public async Task JoinLessonGroup(string lessonId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, lessonId);

        if (!lessonGroups.ContainsKey(lessonId))
            lessonGroups[lessonId] = new List<string>();

        if (!lessonGroups[lessonId].Contains(Context.UserIdentifier))
            lessonGroups[lessonId].Add(Context.UserIdentifier);

        if (lessonGroups[lessonId].Count == 2)
        {
            var users = lessonGroups[lessonId];
            await Clients.User(users[0]).SendAsync("StartCall");
            await Clients.User(users[1]).SendAsync("StartCall");
        }
    }

    public override async Task OnDisconnectedAsync(System.Exception exception)
    {
        foreach (var entry in lessonGroups)
        {
            if (entry.Value.Contains(Context.UserIdentifier))
            {
                entry.Value.Remove(Context.UserIdentifier);
                break;
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendOffer(string toUserId, string offer)
    {
        await Clients.User(toUserId).SendAsync("ReceiveOffer", Context.UserIdentifier, offer);
    }

    public async Task SendAnswer(string toUserId, string answer)
    {
        await Clients.User(toUserId).SendAsync("ReceiveAnswer", Context.UserIdentifier, answer);
    }

    public async Task SendCandidate(string toUserId, string candidate)
    {
        await Clients.User(toUserId).SendAsync("ReceiveCandidate", Context.UserIdentifier, candidate);
    }
}
