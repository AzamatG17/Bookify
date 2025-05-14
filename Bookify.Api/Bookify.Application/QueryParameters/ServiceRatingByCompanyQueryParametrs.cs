namespace Bookify.Application.QueryParameters;

public sealed record ServiceRatingByCompanyQueryParametrs(
    int CompanyId,
    int? BranchId,
    string? SortBy = "idDesc"
    );
