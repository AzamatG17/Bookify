namespace Bookify.Application.Responses;

public sealed record BranchResponse(
    int BranchId,
    string BranchName,
    string BranchAddress,
    BranchCoordinates BranchCoordinates,
    List<BranchOpeningTime> BranchOpeningTime
);

public sealed record BranchCoordinates(
    double? Latitude,
    double? Longitude
);

public sealed record BranchOpeningTime(
    int Day,
    string OpenTime
);

public sealed record NewBranchResponse(
    int BranchId,
    string BranchName,
    string BranchAddress,
    BranchCoordinates BranchCoordinates,
    List<NewBranchOpenHour> BranchOpenHours
);

public sealed record NewBranchOpenHour(
    int Day,
    string OpenTime
);