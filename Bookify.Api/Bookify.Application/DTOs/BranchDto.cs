using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record BranchDto(
    int Id,
    int BranchId,
    string Name,
    string? BranchAddres,
    double? CoordinateLatitude,
    double? CoordinateLongitude,
    Projects Projects,
    List<OpeningTimeDto> OpeningTimeDtos
    );

public record OpeningTimeDto(
    int Day,
    string OpenTime
    );
