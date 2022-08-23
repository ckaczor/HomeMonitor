using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Hub.Service.Hubs
{
    [UsedImplicitly]
    public class DeviceStatusHub : Microsoft.AspNetCore.SignalR.Hub
    {
        [UsedImplicitly]
        public async Task RequestLatestStatus()
        {
            await Clients.Others.SendAsync("RequestLatestStatus");
        }

        [UsedImplicitly]
        public async Task SendLatestStatus(string message)
        {
            Console.WriteLine(message);

            await Clients.Others.SendAsync("LatestStatus", message);
        }
    }
}