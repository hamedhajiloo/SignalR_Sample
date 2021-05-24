using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class MyHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            HubArtifact.OnlineUserCount++;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            HubArtifact.OnlineUserCount--;
            return base.OnDisconnectedAsync(exception);
        }

        public void SendMessage(string message)
        {
            HubArtifact.Messages.Add(message);
        }
    }
}
