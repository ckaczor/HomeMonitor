using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Hub.Service.Hubs
{
    [UsedImplicitly]
    public class PowerHub : Microsoft.AspNetCore.SignalR.Hub
    {
        [UsedImplicitly]
        public async Task SendLatestSample(string message)
        {
            Console.WriteLine(message);

            await Clients.Others.SendAsync("LatestSample", message);
        }
    }
}