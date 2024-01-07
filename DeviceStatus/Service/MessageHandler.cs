using Microsoft.AspNetCore.SignalR.Client;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

namespace Service;

public class MessageHandler : IHostedService
{
    private IMqttClient? _mqttClient;
    private HubConnection? _hubConnection;

    private readonly IConfiguration _configuration;
    private readonly DeviceRepository _deviceRepository;
    private readonly LaundryMonitor _laundryMonitor;
    private readonly Dictionary<string, Timer> _deviceTimers = new();
    private readonly TimeSpan _deviceDelayTime;

    public MessageHandler(IConfiguration configuration, DeviceRepository deviceRepository, LaundryMonitor laundryMonitor)
    {
        _configuration = configuration;
        _deviceRepository = deviceRepository;
        _laundryMonitor = laundryMonitor;

        _deviceDelayTime = TimeSpan.Parse(_configuration["DeviceStatus:DelayTime"]);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_configuration["Hub:DeviceStatus"]))
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(_configuration["Hub:DeviceStatus"]).Build();
            _hubConnection.On("RequestLatestStatus", async () => await RequestLatestStatus());

            await _hubConnection.StartAsync(cancellationToken);
        }

        var mqttFactory = new MqttFactory();

        _mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(_configuration["Mqtt:Server"]).Build();

        _mqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;

        await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("device-status/#");
                })
            .Build();

        await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
    }

    private async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        var topic = arg.ApplicationMessage.Topic;
        var deviceName = topic["device-status/".Length..];
        var payload = arg.ApplicationMessage.ConvertPayloadToString();

        WriteLog($"Topic: {topic} = {payload}");

        var newDevice = new Device(deviceName, payload);

        if (_deviceTimers.ContainsKey(newDevice.Name))
            await _deviceTimers[newDevice.Name].DisposeAsync();

        if (!_deviceRepository.ContainsKey(newDevice.Name) || newDevice.Status)
        {
            WriteLog($"{deviceName}: Handling status immediately");

            await HandleDeviceMessage(newDevice);
        }
        else
        {
            WriteLog($"{deviceName}: Setting timer for status");

            _deviceTimers[newDevice.Name] = new Timer(OnDeviceTimer, newDevice, _deviceDelayTime, Timeout.InfiniteTimeSpan);
        }
    }

    private async Task RequestLatestStatus()
    {
        WriteLog("RequestLatestStatus");

        foreach (var device in _deviceRepository.Values)
        {
            WriteLog($"RequestLatestStatus: {device.Name}");
            await SendDeviceStatus(device);
        }
    }

    private async void OnDeviceTimer(object? state)
    {
        var device = (Device)state!;

        await HandleDeviceMessage(device);

        await _deviceTimers[device.Name].DisposeAsync();

        _deviceTimers.Remove(device.Name);
    }

    private async Task HandleDeviceMessage(Device newDevice)
    {
        if (_deviceRepository.ContainsKey(newDevice.Name) && _deviceRepository[newDevice.Name].Status == newDevice.Status)
        {
            WriteLog($"Skipping device update: {newDevice.Name} = {newDevice.Status}");
            return;
        }

        WriteLog($"Sending device update: {newDevice.Name} = {newDevice.Status}");

        _deviceRepository[newDevice.Name] = newDevice;

        await _laundryMonitor.HandleDeviceMessage(newDevice);

        await SendDeviceStatus(newDevice);
    }

    private async Task SendDeviceStatus(Device device)
    {
        if (_hubConnection == null)
            return;

        try
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
                await _hubConnection.StartAsync();

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

        if (_mqttClient != null)
            await _mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder().Build(), CancellationToken.None);
    }

    private static void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}