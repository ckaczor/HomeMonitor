using RestSharp;

namespace Service;

public class LaundryMonitor
{
    private readonly RestClient _restClient = new();

    private readonly string _botToken;
    private readonly string _chatId;

    public LaundryMonitor(IConfiguration configuration)
    {
        _botToken = configuration["Telegram:BotToken"];
        _chatId = configuration["Telegram:ChatId"];
    }

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