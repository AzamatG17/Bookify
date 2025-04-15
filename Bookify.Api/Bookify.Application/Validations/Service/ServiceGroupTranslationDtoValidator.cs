using Bookify.Application.DTOs;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class ServiceGroupTranslationDtoValidator : AbstractValidator<ServiceGroupTranslationDto>
{
    public ServiceGroupTranslationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя обязательно.")
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов.");

        RuleFor(x => x.LanguageCode)
            .NotEmpty().WithMessage("Язык обязателен.")
            .Must(IsValidLanguage).WithMessage("Язык должен быть валидным ISO-кодом (например, 'en', 'ru', 'uz').");
    }

    private bool IsValidLanguage(string language)
    {
        var validLanguages = new[] { "en", "ru", "uz" };
        return validLanguages.Contains(language.ToLower());
    }
}
