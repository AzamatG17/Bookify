namespace Bookify.Application.Models;

public sealed record Branches(
    int BranchId,
    string BranchName,
    string BranchAddress,
    Coordinates Coordinates,
    List<OpeningTimeDto> OpeningTimeDto
    );

public sealed record OpeningTimeDto(
    int Day,
    string OpenTime
    );

public sealed record Coordinates(
    double? Latitude,
    double? Longitude
    );
