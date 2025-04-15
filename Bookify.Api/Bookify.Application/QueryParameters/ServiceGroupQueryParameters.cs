namespace Bookify.Application.QueryParameters;

public sealed record ServiceGroupQueryParameters(
    string Language,
    int? CompanyId,
    string? SortBy = "idDesc"
    );
