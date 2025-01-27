using Bookify.Domain_.Enums;

namespace Bookify.Application.Requests.Services;

public record CreateCompanyRequest(
    string Name,
    string BaseUrl,
    Projects Projects,
    string LogoBase64,
    string Color,
    string BackgroundColor
    );
