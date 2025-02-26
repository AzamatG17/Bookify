namespace Bookify.Application.DTOs;

public record ServiceDto(
    int Id,
    int ServiceId, 
    string ServiceName,
    string Color,
    int CompanyId,
    string CompanyName, 
    int BranchId,
    string BranchName,
    double? CoordinateLatitude,
    double? CoordinateLongitude);
