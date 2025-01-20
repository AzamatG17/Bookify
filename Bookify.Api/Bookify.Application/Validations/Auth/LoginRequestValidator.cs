using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
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

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required.")
            .Matches(@"^\d{4}$")
            .WithMessage("Code must be a 4-digit number.");
    }
}
