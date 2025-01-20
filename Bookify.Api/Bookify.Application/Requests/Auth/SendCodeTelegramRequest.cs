namespace Bookify.Application.Requests.Auth;

public sealed record SendCodeTelegramRequest(string PhoneNumber, string Language);
