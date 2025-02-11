using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class CreateETicketRequestValidator : AbstractValidator<CreateEticketRequest>
{
    public CreateETicketRequestValidator()
    {
        RuleFor(t => t.ServiceId)
            .GreaterThan(0).WithMessage("ServiceId must be greater than 0.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.")
            .Must(IsValidLanguage).WithMessage("Language must be a valid ISO code (e.g., 'en', 'ru', 'uz').");
    }

    private bool IsValidLanguage(string language)
    {
        var validLanguages = new[] { "en", "ru", "uz" };
        return validLanguages.Contains(language.ToLower());
    }
}
