namespace Bookify.Application.Requests.Auth;

public record GetUserDataForTelegramRequest(string tokenId, int chatId);
