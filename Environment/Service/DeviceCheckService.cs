using ChrisKaczor.HomeMonitor.Environment.Service.Data;

namespace ChrisKaczor.HomeMonitor.Environment.Service;

public class DeviceCheckService(Database database, IConfiguration configuration, TelegramSender telegramSender) : IHostedService
{
    private Timer? _timer;
    private TimeSpan _warningInterval;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        WriteLog("DeviceCheckService started");

        _warningInterval = TimeSpan.Parse(configuration["Environment:DeviceWarningInterval"]!);

        var checkInterval = TimeSpan.Parse(configuration["Environment:DeviceCheckInterval"]!);

        _timer = new Timer(_ => DoWork().Wait(cancellationToken), null, TimeSpan.Zero, checkInterval);

        return Task.CompletedTask;
    }

    private async Task DoWork()
    {
        WriteLog("Checking devices started");

        var devices = await database.GetDevicesAsync();

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
                await database.SetDeviceStoppedReportingAsync(device.Name, true);

                await telegramSender.SendMessageAsync(message);
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