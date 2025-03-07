using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
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

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Требуется код.")
            .Matches(@"^\d{4}$")
            .WithMessage("Код должен состоять из 4 цифр.");
    }
}
