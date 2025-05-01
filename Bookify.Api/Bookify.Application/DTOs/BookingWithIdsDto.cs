namespace Bookify.Application.DTOs;

public record BookingWithIdsDto(
    int Id,
    int BookingId,
    string BookingCode,
    string ServiceName,
    int? BranchId,
    int? SecondBranchId,
    string BranchName,
    DateTime StartDate,
    string StartTime
    );
