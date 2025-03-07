using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class CreateETicketRequestValidator : AbstractValidator<CreateEticketRequest>
{
    public CreateETicketRequestValidator()
    {
        RuleFor(t => t.ServiceId)
            .GreaterThan(0).WithMessage("ServiceId должен быть больше 0.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Язык обязателен.")
            .Must(IsValidLanguage).WithMessage("Язык должен быть валидным ISO-кодом (например, 'en', 'ru', 'uz').");
    }

    private bool IsValidLanguage(string language)
    {
        var validLanguages = new[] { "en", "ru", "uz" };
        return validLanguages.Contains(language.ToLower());
    }
}
