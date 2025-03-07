using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class LoginForTelegramRequestValidator : AbstractValidator<LoginForTelegramRequest>
{
    public LoginForTelegramRequestValidator()
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
            }).
            WithMessage("Номер телефона должен быть в формате +998XXXXXXXXX.");

        RuleFor(x => x.ChatId)
            .GreaterThan(0).WithMessage("Chat ID должен быть положительным числом.");
    }
}
