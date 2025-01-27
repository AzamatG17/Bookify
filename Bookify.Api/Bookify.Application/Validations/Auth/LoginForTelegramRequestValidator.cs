using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class LoginForTelegramRequestValidator : AbstractValidator<LoginForTelegramRequest>
{
    public LoginForTelegramRequestValidator()
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

        RuleFor(x => x.ChatId)
            .GreaterThan(0).WithMessage("Chat ID must be a positive number.");
    }
}
