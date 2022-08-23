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
            Console.WriteLine("RequestLatestStatus");

            await Clients.Others.SendAsync("RequestLatestStatus");
        }

        [UsedImplicitly]
        public async Task SendLatestStatus(string message)
        {
            Console.WriteLine($"LatestStatus: {message}");

            await Clients.Others.SendAsync("LatestStatus", message);
        }
    }
}