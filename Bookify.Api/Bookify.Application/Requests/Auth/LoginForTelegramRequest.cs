namespace Bookify.Application.Requests.Auth;

public record LoginForTelegramRequest(string PhoneNumber, long ChatId);
