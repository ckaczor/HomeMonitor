using ChrisKaczor.HomeMonitor.Power.Service.Data;
using ChrisKaczor.HomeMonitor.Power.Service.Models;
using JetBrains.Annotations;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RestSharp;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Power.Service
{
    [UsedImplicitly]
    public class PowerReader : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly Database _database;
        private readonly TelemetryClient _telemetryClient;

        private HubConnection _hubConnection;
        private Timer _readTimer;

        public PowerReader(IConfiguration configuration, Database database, TelemetryClient telemetryClient)
        {
            _configuration = configuration;
            _database = database;
            _telemetryClient = telemetryClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _telemetryClient.TrackTrace($"{nameof(PowerReader)} - Start");

            _readTimer = new Timer(OnTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            if (!string.IsNullOrEmpty(_configuration["Hub:Power"]))
                _hubConnection = new HubConnectionBuilder().WithUrl(_configuration["Hub:Power"]).Build();

            return Task.CompletedTask;
        }

        private void OnTimer(object state)
        {
            try
            {
                var client = new RestClient(_configuration["Power:Host"]);

                var request = new RestRequest("current-sample", Method.GET);
                request.AddHeader("Authorization", _configuration["Power:AuthorizationHeader"]);

                var response = client.Execute(request);

                var sample = JsonSerializer.Deserialize<PowerSample>(response.Content);

                var generation = Array.Find(sample.Channels, c => c.Type == "GENERATION");
                var consumption = Array.Find(sample.Channels, c => c.Type == "CONSUMPTION");

                if (generation == null || consumption == null)
                    return;

                var status = new PowerStatus { Generation = generation.RealPower, Consumption = consumption.RealPower };

                _database.StorePowerData(status);

                var json = JsonSerializer.Serialize(status);

                Console.WriteLine(json);

                if (_hubConnection == null)
                    return;

                if (_hubConnection.State == HubConnectionState.Disconnected)
                    _hubConnection.StartAsync().Wait();

                _hubConnection.InvokeAsync("SendLatestSample", json).Wait();
            }
            catch (Exception exception)
            {
                WriteLog($"Exception: {exception}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _telemetryClient.TrackTrace($"{nameof(PowerReader)} - Stop");

            _readTimer.Dispose();

            _hubConnection?.StopAsync(cancellationToken).Wait(cancellationToken);

            return Task.CompletedTask;
        }

        private static void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
    }
}