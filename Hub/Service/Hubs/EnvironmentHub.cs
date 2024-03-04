using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Hub.Service.Hubs
{
    [UsedImplicitly]
    public class EnvironmentHub(ILogger<EnvironmentHub> logger) : Microsoft.AspNetCore.SignalR.Hub
    {
        [UsedImplicitly]
        public async Task SendLatest(string message)
        {
            logger.LogInformation(message);

            await Clients.Others.SendAsync("Latest", message);
        }

        [UsedImplicitly]
        public async Task RequestLatest()
        {
            logger.LogInformation("RequestLatest");

            await Clients.Others.SendAsync("RequestLatest");
        }
    }
}