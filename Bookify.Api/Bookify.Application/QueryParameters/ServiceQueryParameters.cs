﻿namespace Bookify.Application.QueryParameters;

public sealed record ServiceQueryParameters(
    string Language,
    int? CompanyId,
    int? BranchId,
    int? ServiceGroupId,
    string? Search,
    string? SortBy = "idDesc"
    );
