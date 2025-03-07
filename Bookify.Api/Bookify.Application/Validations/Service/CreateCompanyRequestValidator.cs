using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название обязательно.")
            .MaximumLength(100)
            .WithMessage("Название не должно превышать 100 символов.");

        RuleFor(x => x.LogoBase64)
            .NotNull()
            .WithMessage("Логотип не может быть null.");

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Цвет обязателен.")
            .Matches("^#[0-9a-fA-F]{6}$")
            .WithMessage("Цвет должен быть валидным шестнадцатеричным кодом (например, #FFFFFF).");

        RuleFor(x => x.BackgroundColor)
            .NotEmpty()
            .WithMessage("Фоновый цвет обязателен.")
            .Matches("^#[0-9a-fA-F]{6}$")
            .WithMessage("Фоновый цвет должен быть валидным шестнадцатеричным кодом (например, #FFFFFF).");
    }
}
