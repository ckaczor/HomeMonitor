using RestSharp;

namespace ChrisKaczor.HomeMonitor.Environment.Service;

public class TelegramSender(IConfiguration configuration)
{
    private readonly string _botToken = configuration["Telegram:BotToken"]!;
    private readonly string _chatId = configuration["Telegram:PersonalChatId"]!;
    private readonly RestClient _restClient = new();

    public async Task SendMessageAsync(string message)
    {
        var encodedMessage = Uri.EscapeDataString(message);

        var restRequest = new RestRequest($"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={_chatId}&text={encodedMessage}");

        await _restClient.GetAsync(restRequest);
    }
}