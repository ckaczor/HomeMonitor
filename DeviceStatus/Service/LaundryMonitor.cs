using RestSharp;

namespace ChrisKaczor.HomeMonitor.DeviceStatus.Service;

public class LaundryMonitor(IConfiguration configuration)
{
    private readonly string _botToken = configuration["Telegram:BotToken"]!;
    private readonly string _chatId = configuration["Telegram:ChatId"]!;
    private readonly RestClient _restClient = new();

    public async Task HandleDeviceMessage(Device device)
    {
        try
        {
            if (device.Name is not ("washer" or "dryer"))
                return;

            var status = device.Status ? "ON" : "OFF";

            var message = $"The {device.Name} is now {status}.";

            var encodedMessage = Uri.EscapeDataString(message);

            var restRequest = new RestRequest($"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={_chatId}&text={encodedMessage}");

            await _restClient.GetAsync(restRequest);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}