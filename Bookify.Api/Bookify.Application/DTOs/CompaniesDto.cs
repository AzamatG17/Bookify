using Bookify.Domain_.Enums;

namespace Bookify.Application.DTOs;

public record CompaniesDto(
    int Id,
    string Name,
    string LogoBase64,
    string? Color,
    string? BackgroundColor
    );
