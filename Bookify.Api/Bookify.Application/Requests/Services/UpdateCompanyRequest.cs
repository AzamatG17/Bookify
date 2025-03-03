namespace Bookify.Application.Requests.Services;

public record UpdateCompanyRequest(
    int Id,
    string Name,
    string? BaseUrlForOnlinet,
    string? BaseUrlForBookingService,
    string LogoBase64,
    string Color,
    string BackgroundColor
    );
