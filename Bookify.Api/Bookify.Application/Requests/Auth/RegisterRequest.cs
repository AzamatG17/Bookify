namespace Bookify.Application.Requests.Auth;

public sealed record RegisterRequest(
    string PhoneNumber,
    string FirstName,
    string LastName,
    long ChatId
    );
