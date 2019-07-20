using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using ChrisKaczor.HomeMonitor.Weather.Models;

namespace ChrisKaczor.HomeMonitor.Weather.SerialReader
{
    internal static class Program
    {
        private static IConfiguration _configuration;

        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        private static DateTime _lastLogTime = DateTime.MinValue;
        private static long _messageCount;
        private static bool _boardStarting;

        private static void Main()
        {
            WriteLog("Starting");

            Console.CancelKeyPress += OnCancelKeyPress;

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .AddEnvironmentVariables()
                .Build();

            var baudRate = int.Parse(_configuration["Weather:Port:BaudRate"]);

            while (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    WriteLog("Starting main loop");

                    Thread.Sleep(1000);

                    var port = GetPort();

                    if (port == null)
                        continue;

                    using var serialPort = new SerialPort(port, baudRate) { NewLine = "\r\n" };

                    WriteLog("Opening serial port");

                    serialPort.Open();

                    _boardStarting = false;

                    var factory = new ConnectionFactory
                    {
                        HostName = _configuration["Weather:Queue:Host"],
                        UserName = _configuration["Weather:Queue:User"],
                        Password = _configuration["Weather:Queue:Password"]
                    };

                    WriteLog("Connecting to queue server");

                    using var connection = factory.CreateConnection();
                    using var model = connection.CreateModel();

                    WriteLog("Declaring queue");

                    model.QueueDeclare(_configuration["Weather:Queue:Name"], true, false, false, null);

                    WriteLog("Starting serial read loop");

                    ReadSerial(serialPort, model);
                }
                catch (Exception exception)
                {
                    WriteLog($"Exception: {exception}");
                }
            }

            WriteLog("Exiting");
        }

        private static void ReadSerial(SerialPort serialPort, IModel model)
        {
            while (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var message = serialPort.ReadLine();

                    if (!_boardStarting)
                    {
                        _boardStarting = message.Contains("Board starting");

                        if (_boardStarting)
                            WriteLog("Board starting");
                    }

                    if (!_boardStarting)
                    {
                        WriteLog($"Message received but board not starting: {message}");
                        continue;
                    }

                    var weatherMessage = WeatherMessage.Parse(message);

                    var messageString = JsonConvert.SerializeObject(weatherMessage);

                    var body = Encoding.UTF8.GetBytes(messageString);

                    var properties = model.CreateBasicProperties();

                    properties.Persistent = true;

                    model.BasicPublish(string.Empty, _configuration["Weather:Queue:Name"], properties, body);

                    _messageCount++;

                    if ((DateTime.Now - _lastLogTime).TotalMinutes < 1)
                        continue;

                    WriteLog($"Number of messages received since {_lastLogTime} = {_messageCount}");

                    _lastLogTime = DateTime.Now;
                    _messageCount = 0;
                }
                catch (TimeoutException)
                {
                    WriteLog("Serial port read timeout");
                }
            }
        }

        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            CancellationTokenSource.Cancel();
        }

        private static string GetPort()
        {
            var portPrefix = _configuration["Weather:Port:Prefix"];

            while (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                WriteLog($"Checking for port starting with: {portPrefix}");

                var ports = SerialPort.GetPortNames();

                var port = ports.FirstOrDefault(p => p.StartsWith(portPrefix));

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

        private static void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
    }
}