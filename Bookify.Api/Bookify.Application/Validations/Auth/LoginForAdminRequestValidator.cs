using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class LoginForAdminRequestValidator : AbstractValidator<LoginForAdminRequest>
{
    public LoginForAdminRequestValidator()
    {
        RuleFor(request => request.Login)
            .NotEmpty().WithMessage("Login is required.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
