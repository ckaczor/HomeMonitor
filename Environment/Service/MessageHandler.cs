using System.Text.Json;
using ChrisKaczor.HomeMonitor.Environment.Service.Data;
using Microsoft.AspNetCore.SignalR.Client;
using MQTTnet;
using MQTTnet.Client;

namespace ChrisKaczor.HomeMonitor.Environment.Service;

public class MessageHandler : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly Database _database;
    private readonly IMqttClient _mqttClient;

    private readonly MqttFactory _mqttFactory;
    private readonly string _topic;
    private readonly HubConnection? _hubConnection;

    public MessageHandler(IConfiguration configuration, Database database)
    {
        _configuration = configuration;
        _database = database;

        _database.EnsureDatabase();

        _topic = _configuration["Mqtt:Topic"] ?? string.Empty;

        if (string.IsNullOrEmpty(_topic))
            throw new InvalidOperationException("Topic not set");

        _mqttFactory = new MqttFactory();
        _mqttClient = _mqttFactory.CreateMqttClient();

        _mqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;

        var hubUrl = configuration["Environment:Hub:Url"];

        if (!string.IsNullOrEmpty(hubUrl))
            _hubConnection = new HubConnectionBuilder().WithUrl(hubUrl).Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_hubConnection != null)
            await _hubConnection.StartAsync(cancellationToken);

        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(_configuration["Mqtt:Server"]).Build();
        await _mqttClient.ConnectAsync(mqttClientOptions, cancellationToken);

        var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter($"{_topic}/#").Build();
        await _mqttClient.SubscribeAsync(mqttSubscribeOptions, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder().Build(), cancellationToken);

        if (_hubConnection != null)
            await _hubConnection.StopAsync(cancellationToken);
    }

    private async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        var topic = arg.ApplicationMessage.Topic;
        var payload = arg.ApplicationMessage.ConvertPayloadToString();

        WriteLog($"Topic: {topic} = {payload}");

        var message = JsonSerializer.Deserialize<Message>(payload);

        if (message == null)
            return;

        await _database.StoreMessageAsync(message);

        await SendMessage(message);
    }

    private async Task SendMessage(Message message)
    {
        try
        {
            if (_hubConnection == null)
                return;

            if (_hubConnection.State == HubConnectionState.Disconnected)
                await _hubConnection.StartAsync();

            var json = JsonSerializer.Serialize(message);

            await _hubConnection.InvokeAsync("SendMessage", json);
        }
        catch (Exception exception)
        {
            WriteLog($"Hub exception: {exception}");
        }
    }

    private static void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}