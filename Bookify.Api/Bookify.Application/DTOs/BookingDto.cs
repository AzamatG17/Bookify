namespace Bookify.Application.DTOs;

public record BookingDto(
    string BookingCode,
    string ServiceName,
    string BranchName,
    DateTime StartDate,
    string StartTime
    );
