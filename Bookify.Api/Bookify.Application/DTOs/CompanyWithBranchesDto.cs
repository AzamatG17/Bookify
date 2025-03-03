using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record CompanyWithBranchesDto(
    int Id,
    string Name,
    string? Color,
    string? BackgroundColor,
    List<BranchDto>? Branches
    );
