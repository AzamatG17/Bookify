using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class DeleteEticketRequestValidator : AbstractValidator<DeleteEticketRequest>
{
    public DeleteEticketRequestValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Номер билета обязателен.");

        RuleFor(x => x.SecondBranchId)
            .GreaterThan(0).WithMessage("ID второй ветки должен быть больше нуля.");

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
