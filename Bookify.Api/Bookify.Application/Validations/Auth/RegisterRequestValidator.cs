using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона обязателен.")
            .Must((request, phoneNumber, context) =>
            {
                if (phoneNumber!.Length == 9)
                {
                    return phoneNumber.All(char.IsDigit);
                }

                if (phoneNumber.Length == 13 && phoneNumber.StartsWith("+998"))
                {
                    return phoneNumber[4..].All(char.IsDigit);
                }

                return false;
            }).WithMessage("Номер телефона должен быть в формате +998XXXXXXXXX.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно.")
            .MaximumLength(50).WithMessage("Имя не должно превышать 50 символов.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна.")
            .MaximumLength(50).WithMessage("Фамилия не должна превышать 50 символов.");

        RuleFor(x => x.ChatId)
            .GreaterThan(0).WithMessage("Chat ID должен быть положительным числом.");

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
