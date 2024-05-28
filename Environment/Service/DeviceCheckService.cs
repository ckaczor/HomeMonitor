using ChrisKaczor.HomeMonitor.Environment.Service.Data;
using RestSharp;

namespace ChrisKaczor.HomeMonitor.Environment.Service;

public class DeviceCheckService(Database _database, IConfiguration _configuration) : IHostedService
{
    private Timer? _timer;
    private TimeSpan _warningInterval;

    private readonly string _botToken = _configuration["Telegram:BotToken"]!;
    private readonly string _chatId = _configuration["Telegram:ChatId"]!;
    private readonly RestClient _restClient = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        WriteLog("DeviceCheckService started");

        _warningInterval = TimeSpan.Parse(_configuration["Environment:DeviceWarningInterval"]!);

        var checkInterval = TimeSpan.Parse(_configuration["Environment:DeviceCheckInterval"]!);

        _timer = new Timer((state) => DoWork().Wait(), null, TimeSpan.Zero, checkInterval);

        return Task.CompletedTask;
    }

    private async Task DoWork()
    {
        WriteLog("Checking devices started");

        var devices = await _database.GetDevicesAsync();

        foreach (var device in devices)
        {
            WriteLog($"Checking device: {device.Name}");

            var message = string.Empty;

            WriteLog($"Device {device.Name} last updated: {(device.LastUpdated == null ? "never" : device.LastUpdated.ToString())}");

            if (device.LastUpdated == null)
            {
                message = $"Device has never reported: {device.Name}";
            }
            else if (DateTime.UtcNow - device.LastUpdated > _warningInterval)
            {
                message = $"Device has not reported recently: {device.Name}";
            }

            if (message.Length > 0)
            {
                var encodedMessage = Uri.EscapeDataString(message);

                var restRequest = new RestRequest($"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={_chatId}&text={encodedMessage}");

                await _restClient.GetAsync(restRequest);
            }
        }

        WriteLog("Checking devices finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        WriteLog("DeviceCheckService stopped");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private static void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}