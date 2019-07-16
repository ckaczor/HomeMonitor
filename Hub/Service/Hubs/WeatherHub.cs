using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Hub.Service.Hubs
{
    [UsedImplicitly]
    public class WeatherHub : Microsoft.AspNetCore.SignalR.Hub
    {
        [UsedImplicitly]
        public async Task SendLatestReading(string message)
        {
            Console.WriteLine(message);

            await Clients.Others.SendAsync("LatestReading", message);
        }
    }
}