using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class DeleteEticketRequestValidator : AbstractValidator<DeleteEticketRequest>
{
    public DeleteEticketRequestValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Ticket number is required.");

        RuleFor(x => x.SecondBranchId)
            .GreaterThan(0).WithMessage("Second branch ID must be greater than zero.");

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
