using Bookify.Application.Configurations;
using Bookify.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.SendNotification;

internal sealed class TelegramService : ITelegramService
{
    private readonly HttpClient _client;
    private readonly BotTokenOptions _options;

    public TelegramService(HttpClient client, IOptionsMonitor<BotTokenOptions> options)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options.CurrentValue ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task SendMessageAsync(long chatId, string text)
    {
        var url = $"https://api.telegram.org/bot{_options.TelegramToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(text)}";

        var response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ошибка при отправке сообщения: {error}");
        }
    }
}
