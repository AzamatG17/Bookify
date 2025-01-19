using Bookify.Domain_.Enums;

namespace Bookify.Application.Requests.Auth;

public sealed record RegisterRequest(
    string PhoneNumber,
    string FirstName,
    string LastName,
    string? Email,
    DateTime BirthDate,
    Gender? Gender,
    string SmsCode
    );
