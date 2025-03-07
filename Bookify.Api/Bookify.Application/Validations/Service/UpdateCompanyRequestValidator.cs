using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("CompanyId должен быть больше 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название обязательно для заполнения.")
            .MaximumLength(100)
            .WithMessage("Название не должно превышать 100 символов.");

        RuleFor(x => x.LogoBase64)
            .NotNull()
            .WithMessage("Логотип не может быть null.");

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Цвет обязателен для заполнения.")
            .Matches("^#[0-9a-fA-F]{6}$")
            .WithMessage("Цвет должен быть в формате шестнадцатеричного кода (например, #FFFFFF).");

        RuleFor(x => x.BackgroundColor)
            .NotEmpty()
            .WithMessage("Фон обязателен для заполнения.")
            .Matches("^#[0-9a-fA-F]{6}$")
            .WithMessage("Фон должен быть в формате шестнадцатеричного кода (например, #FFFFFF).");
    }
}
