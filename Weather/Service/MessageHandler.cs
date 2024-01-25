using ChrisKaczor.HomeMonitor.Weather.Models;
using ChrisKaczor.HomeMonitor.Weather.Service.Data;
using ChrisKaczor.HomeMonitor.Weather.Service.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Weather.Service;

[UsedImplicitly]
public class MessageHandler(IConfiguration configuration, Database database) : IHostedService
{
    private IConnection _queueConnection;
    private IModel _queueModel;

    private HubConnection _hubConnection;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var host = configuration["Weather:Queue:Host"];

        if (string.IsNullOrEmpty(host))
            return Task.CompletedTask;

        WriteLog("MessageHandler: Start");

        var factory = new ConnectionFactory
        {
            HostName = host,
            UserName = configuration["Weather:Queue:User"],
            Password = configuration["Weather:Queue:Password"]
        };

        _queueConnection = factory.CreateConnection();
        _queueModel = _queueConnection.CreateModel();

        _queueModel.QueueDeclare(configuration["Weather:Queue:Name"], true, false, false, null);

        var consumer = new EventingBasicConsumer(_queueModel);
        consumer.Received += DeviceEventHandler;

        _queueModel.BasicConsume(configuration["Weather:Queue:Name"], true, consumer);

        if (!string.IsNullOrEmpty(configuration["Hub:Weather"]))
            _hubConnection = new HubConnectionBuilder().WithUrl(configuration["Hub:Weather"]).Build();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        WriteLog("MessageHandler: Stop");

        _hubConnection?.StopAsync(cancellationToken).Wait(cancellationToken);

        _queueModel?.Close();
        _queueConnection?.Close();

        return Task.CompletedTask;
    }

    private void DeviceEventHandler(object model, BasicDeliverEventArgs eventArgs)
    {
        try
        {
            var body = eventArgs.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            WriteLog($"Message received: {message}");

            var weatherMessage = JsonConvert.DeserializeObject<WeatherMessage>(message);

            if (weatherMessage.Type == MessageType.Text)
            {
                WriteLog(weatherMessage.Message);

                return;
            }

            database.StoreWeatherData(weatherMessage);

            if (_hubConnection == null)
                return;

            var weatherUpdate = new WeatherUpdate(weatherMessage, database);

            try
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                    _hubConnection.StartAsync().Wait();

                var json = JsonConvert.SerializeObject(weatherUpdate);

                _hubConnection.InvokeAsync("SendLatestReading", json).Wait();
            }
            catch (Exception exception)
            {
                WriteLog($"Hub exception: {exception}");
            }
        }
        catch (Exception exception)
        {
            WriteLog($"Exception: {exception}");

            throw;
        }
    }

    private static void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}