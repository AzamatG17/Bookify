using Bookify.Application.Requests.Auth;
using FluentValidation;

namespace Bookify.Application.Validations.Auth;

public class LoginForAdminRequestValidator : AbstractValidator<LoginForAdminRequest>
{
    public LoginForAdminRequestValidator()
    {
        RuleFor(request => request.Login)
            .NotEmpty().WithMessage("Логин обязателен для заполнения.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения.");
    }
}
