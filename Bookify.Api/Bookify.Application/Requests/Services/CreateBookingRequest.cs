namespace Bookify.Application.Requests.Services;

public record CreateBookingRequest(
    int ServiceId,
    DateTime StartDate,
    string StartTime,
    string Language
    );
