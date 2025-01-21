using Bookify.Domain_.Enums;

namespace Bookify.Application.Requests.Services;

public record CreateCompanyRequest(
    string Name,
    string BaseUrl,
    Projects Projects,
    string Color,
    string BackgroundColor
    );
