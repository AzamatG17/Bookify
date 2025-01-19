namespace Bookify.Application.Requests.Auth;

public sealed record LoginRequest(string PhoneNumber, string Code);
