namespace Bookify.Application.Requests.Services;

public record CreateEticketRequest(
    int ServiceId,
    string LanguageId
    );
