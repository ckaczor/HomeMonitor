using ChrisKaczor.HomeMonitor.Weather.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ChrisKaczor.HomeMonitor.Weather.SerialReader;

public class SerialReader(IConfiguration configuration, ILogger<SerialReader> logger) : IHostedService
{
    public static bool BoardStarted { get; private set; }
    public static DateTimeOffset LastReading { get; private set; }

    private CancellationToken _cancellationToken;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;

        Task.Run(Execute, cancellationToken);

        return Task.CompletedTask;
    }

    private void Execute()
    {
        var baudRate = configuration.GetValue<int>("Weather:Port:BaudRate");

        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                WriteLog("Starting main loop");

                Thread.Sleep(1000);

                var port = GetPort();

                if (port == null)
                    continue;

                using var serialPort = new SerialPort(port, baudRate);

                serialPort.NewLine = "\r\n";

                WriteLog("Opening serial port");

                serialPort.Open();

                BoardStarted = false;

                var factory = new ConnectionFactory
                {
                    HostName = configuration["Weather:Queue:Host"],
                    UserName = configuration["Weather:Queue:User"],
                    Password = configuration["Weather:Queue:Password"]
                };

                WriteLog("Connecting to queue server");

                using var connection = factory.CreateConnection();
                using var model = connection.CreateModel();

                WriteLog("Declaring queue");

                model.QueueDeclare(configuration["Weather:Queue:Name"], true, false, false, null);

                WriteLog("Starting serial read loop");

                ReadSerial(serialPort, model);
            }
            catch (Exception exception)
            {
                WriteLog($"Exception: {exception}");
            }
        }
    }

    private void ReadSerial(SerialPort serialPort, IModel model)
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = serialPort.ReadLine();

                if (!BoardStarted)
                {
                    BoardStarted = message.Contains("Board starting");

                    if (BoardStarted)
                        WriteLog("Board started");
                }

                if (!BoardStarted)
                {
                    WriteLog($"Message received but board not started: {message}");
                    continue;
                }

                LastReading = DateTimeOffset.UtcNow;

                WriteLog($"Message received: {message}");

                var weatherMessage = WeatherMessage.Parse(message);

                var messageString = JsonConvert.SerializeObject(weatherMessage);

                var body = Encoding.UTF8.GetBytes(messageString);

                var properties = model.CreateBasicProperties();

                properties.Persistent = true;

                model.BasicPublish(string.Empty, configuration["Weather:Queue:Name"], properties, body);
            }
            catch (TimeoutException)
            {
                WriteLog("Serial port read timeout");
            }
        }
    }

    private string GetPort()
    {
        var portPrefix = configuration["Weather:Port:Prefix"]!;

        while (!_cancellationToken.IsCancellationRequested)
        {
            WriteLog($"Checking for port starting with: {portPrefix}");

            var ports = SerialPort.GetPortNames();

            var port = Array.Find(ports, p => p.StartsWith(portPrefix));

            if (port != null)
            {
                WriteLog($"Port found: {port}");
                return port;
            }

            WriteLog("Port not found - waiting");

            Thread.Sleep(1000);
        }

        return null;
    }

    private void WriteLog(string message)
    {
        logger.LogInformation(message);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}