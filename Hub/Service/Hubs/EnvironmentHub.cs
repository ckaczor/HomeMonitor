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
        public async Task SendMessage(string message)
        {
            logger.LogInformation(message);

            await Clients.Others.SendAsync("Message", message);
        }
    }
}