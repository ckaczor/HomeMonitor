using ChrisKaczor.HomeMonitor.Power.Service.Data;
using ChrisKaczor.HomeMonitor.Power.Service.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Power.Service;

[UsedImplicitly]
public class PowerReader(IConfiguration configuration, Database database, ILogger<PowerReader> logger) : IHostedService
{
    private readonly ActivitySource _activitySource = new(nameof(PowerReader));

    private HubConnection _hubConnection;
    private Timer _readTimer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(PowerReader)} - Start");

        _readTimer = new Timer(GetCurrentSample, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        if (!string.IsNullOrEmpty(configuration["Hub:Power"]))
            _hubConnection = new HubConnectionBuilder().WithUrl(configuration["Hub:Power"]).Build();

        return Task.CompletedTask;
    }

    private void GetCurrentSample(object state)
    {
        using var activity = _activitySource.StartActivity();

        try
        {
            var client = new RestClient(configuration["Power:Host"]!);

            var request = new RestRequest("current-sample");
            request.AddHeader("Authorization", configuration["Power:AuthorizationHeader"]!);

            var response = client.Execute(request);

            var content = response.Content!;

            logger.LogInformation("API response: {content}", content);

            var sample = JsonSerializer.Deserialize<PowerSample>(content);

            var generation = Array.Find(sample.Channels, c => c.Type == "GENERATION");
            var consumption = Array.Find(sample.Channels, c => c.Type == "CONSUMPTION");

            if (generation == null || consumption == null)
                return;

            var status = new PowerStatus { Generation = generation.RealPower, Consumption = consumption.RealPower };

            database.StorePowerData(status);

            var json = JsonSerializer.Serialize(status);

            logger.LogInformation("Output message: {json}", json);

            if (_hubConnection == null)
                return;

            if (_hubConnection.State == HubConnectionState.Disconnected)
                _hubConnection.StartAsync().Wait();

            _hubConnection.InvokeAsync("SendLatestSample", json).Wait();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(PowerReader)} - Stop");

        _readTimer.Dispose();

        _hubConnection?.StopAsync(cancellationToken).Wait(cancellationToken);

        return Task.CompletedTask;
    }
}