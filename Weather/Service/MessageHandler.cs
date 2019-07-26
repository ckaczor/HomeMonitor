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
using ChrisKaczor.HomeMonitor.Weather.Models;
using ChrisKaczor.HomeMonitor.Weather.Service.Data;

namespace ChrisKaczor.HomeMonitor.Weather.Service
{
    [UsedImplicitly]
    public class MessageHandler : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly Database _database;

        private IConnection _queueConnection;
        private IModel _queueModel;

        private HubConnection _hubConnection;

        private static DateTime _lastLogTime = DateTime.MinValue;
        private static long _messageCount;

        public MessageHandler(IConfiguration configuration, Database database)
        {
            _configuration = configuration;
            _database = database;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteLog("MessageHandler: Start");

            var factory = new ConnectionFactory
            {
                HostName = _configuration["Weather:Queue:Host"],
                UserName = _configuration["Weather:Queue:User"],
                Password = _configuration["Weather:Queue:Password"]
            };

            _queueConnection = factory.CreateConnection();
            _queueModel = _queueConnection.CreateModel();

            _queueModel.QueueDeclare(_configuration["Weather:Queue:Name"], true, false, false, null);

            var consumer = new EventingBasicConsumer(_queueModel);
            consumer.Received += DeviceEventHandler;

            _queueModel.BasicConsume(_configuration["Weather:Queue:Name"], true, consumer);

            _hubConnection = new HubConnectionBuilder().WithUrl(_configuration["Hub:Weather"]).Build();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteLog("MessageHandler: Stop");

            _hubConnection.StopAsync(cancellationToken).Wait(cancellationToken);

            _queueModel.Close();
            _queueConnection.Close();

            return Task.CompletedTask;
        }

        private void DeviceEventHandler(object model, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var body = eventArgs.Body;
                var message = Encoding.UTF8.GetString(body);

                _messageCount++;

                if ((DateTime.Now - _lastLogTime).TotalMinutes >= 1)
                {
                    WriteLog($"Number of messages received since {_lastLogTime} = {_messageCount}");

                    _lastLogTime = DateTime.Now;
                    _messageCount = 0;
                }

                var weatherMessage = JsonConvert.DeserializeObject<WeatherMessage>(message);

                if (weatherMessage.Type == MessageType.Text)
                {
                    WriteLog(weatherMessage.Message);

                    return;
                }

                _database.StoreWeatherData(weatherMessage);

                if (_hubConnection.State == HubConnectionState.Disconnected)
                    _hubConnection.StartAsync().Wait();

                _hubConnection.InvokeAsync("SendLatestReading", message).Wait();
            }
            catch (Exception exception)
            {
                WriteLog($"Exception: {exception}");
            }
        }

        private static void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
    }
}