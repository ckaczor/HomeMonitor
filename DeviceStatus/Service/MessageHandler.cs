using Microsoft.AspNetCore.SignalR.Client;
using MQTTnet;
using MQTTnet.Server;
using System.Text.Json;

namespace Service;

public class MessageHandler : IHostedService
{
    private MqttServer? _mqttServer;
    private HubConnection? _hubConnection;

    private readonly IConfiguration _configuration;
    private readonly DeviceRepository _deviceRepository;
    private readonly LaundryMonitor _laundryMonitor;

    public MessageHandler(IConfiguration configuration, DeviceRepository deviceRepository, LaundryMonitor laundryMonitor)
    {
        _configuration = configuration;
        _deviceRepository = deviceRepository;
        _laundryMonitor = laundryMonitor;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_configuration["Hub:DeviceStatus"]))
            _hubConnection = new HubConnectionBuilder().WithUrl(_configuration["Hub:DeviceStatus"]).Build();

        var mqttFactory = new MqttFactory();

        var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

        _mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions);
        _mqttServer.InterceptingPublishAsync += OnInterceptingPublishAsync;

        await _mqttServer.StartAsync();
    }

    private async Task OnInterceptingPublishAsync(InterceptingPublishEventArgs arg)
    {
        WriteLog($"{arg.ApplicationMessage.Topic}: {arg.ApplicationMessage.ConvertPayloadToString()}");

        _deviceRepository.HandleDeviceMessage(arg.ApplicationMessage.Topic, arg.ApplicationMessage.ConvertPayloadToString());

        var device = _deviceRepository[arg.ApplicationMessage.Topic];

        await _laundryMonitor.HandleDeviceMessage(device);

        if (_hubConnection == null)
            return;

        try
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
                _hubConnection.StartAsync().Wait();

            var json = JsonSerializer.Serialize(device);

            await _hubConnection.InvokeAsync("SendLatestStatus", json);
        }
        catch (Exception exception)
        {
            WriteLog($"Hub exception: {exception}");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_hubConnection != null)
            await _hubConnection.StopAsync(cancellationToken);

        if (_mqttServer != null)
            await _mqttServer.StopAsync();
    }

    private static void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}