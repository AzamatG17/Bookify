namespace Bookify.Application.DTOs;

public record BookingDto(
    string BookingCode,
    string ServiceName,
    int? BranchId,
    int? SecondBranchId,
    string BranchName,
    DateTime StartDate,
    string StartTime
    );
