namespace Bookify.Application.QueryParameters;

public sealed record ServiceQueryParameters(
    string Language,
    int? CompanyId,
    string? Search,
    string? SortBy = "idDesc"
    );
