using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Hub.Service.Hubs
{
    [UsedImplicitly]
    public class DeviceStatusHub(ILogger<DeviceStatusHub> logger) : Microsoft.AspNetCore.SignalR.Hub
    {
        [UsedImplicitly]
        public async Task RequestLatestStatus()
        {
            logger.LogInformation("RequestLatestStatus");

            await Clients.Others.SendAsync("RequestLatestStatus");
        }

        [UsedImplicitly]
        public async Task SendLatestStatus(string message)
        {
            logger.LogInformation($"LatestStatus: {message}");

            await Clients.Others.SendAsync("LatestStatus", message);
        }
    }
}