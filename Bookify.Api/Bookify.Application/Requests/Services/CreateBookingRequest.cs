namespace Bookify.Application.Requests.Services;

public record CreateBookingRequest(
    int ServiceId,
    Guid UserId,
    string StartDate,
    string StartTime,
    string Language
    );
