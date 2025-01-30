namespace Bookify.Application.Requests.Services;

public record CreateBookingRequest(
    int ServiceId,
    Guid UserId,
    DateTime StartDate,
    string StartTime,
    string Language
    );
