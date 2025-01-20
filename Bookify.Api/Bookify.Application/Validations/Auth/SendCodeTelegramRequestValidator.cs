using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class SendCodeTelegramRequestValidator : AbstractValidator<SendCodeTelegramRequest>
{
    public SendCodeTelegramRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
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
            }).
            WithMessage("Phone number must be in the format +998XXXXXXXXX.");

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
