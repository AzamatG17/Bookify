namespace Bookify.Application.DTOs;

public record CompaniesForAdminDto(
    int Id,
    string Name,
    string? BaseUrlForOnlinet,
    string? BaseUrlForBookingService,
    string LogoBase64,
    string? Color,
    string? BackgroundColor
    );
