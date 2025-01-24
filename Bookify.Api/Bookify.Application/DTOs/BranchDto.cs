namespace Bookify.Application.DTOs;

public record BranchDto(
    int BranchId,
    string Name,
    string? BranchAddres,
    double? CoordinateLatitude,
    double? CoordinateLongitude,
    List<OpeningTimeDto> OpeningTimeDtos
    );

public record OpeningTimeDto(
    int Day,
    string OpenTime
    );
