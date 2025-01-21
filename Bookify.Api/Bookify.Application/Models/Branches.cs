namespace Bookify.Application.Models;

public sealed record Branches(
    int BranchId,
    string BranchName,
    string BranchAddress,
    string Latitude,
    string Longitude,
    List<OpeningTimeDto> OpeningTimeDto
    );

public sealed record OpeningTimeDto(
    int Day,
    string OpenTime
    );
