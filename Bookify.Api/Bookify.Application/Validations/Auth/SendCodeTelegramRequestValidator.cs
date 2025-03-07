using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class SendCodeTelegramRequestValidator : AbstractValidator<SendCodeTelegramRequest>
{
    public SendCodeTelegramRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Номер телефона обязателен.")
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
