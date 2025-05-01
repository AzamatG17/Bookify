namespace Bookify.Application.DTOs;

public record ServiceWithRatingDto(
    int Id,
    int ServiceId,
    string ServiceName,
    string Color,
    int CompanyId,
    string CompanyName,
    int BranchId,
    string BranchName,
    double? Rating,
    double? CoordinateLatitude,
    double? CoordinateLongitude);
