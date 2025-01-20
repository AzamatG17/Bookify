namespace Bookify.Application.Interfaces;

public interface ITelegramService
{
    Task SendMessageAsync(long chatId, string text);

}
