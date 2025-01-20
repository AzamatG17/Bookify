using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
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

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.ChatId)
            .GreaterThan(0).WithMessage("Chat ID must be a positive number.");
    }
}
