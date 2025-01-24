using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record CompanyWithBranchesDto(
    int Id,
    string Name,
    Projects Projects,
    string? Color,
    string? BackgroundColor,
    List<BranchDto>? Branches
    );
