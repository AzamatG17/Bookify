using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.LogoBase64)
            .NotNull()
            .WithMessage("Logo cannot be null.");

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Color is required.")
            .Matches("^#[0-9a-fA-F]{6}$")
            .WithMessage("Color must be a valid hex color code (e.g., #FFFFFF).");

        RuleFor(x => x.BackgroundColor)
            .NotEmpty()
            .WithMessage("BackgroundColor is required.")
            .Matches("^#[0-9a-fA-F]{6}$")
            .WithMessage("BackgroundColor must be a valid hex color code (e.g., #FFFFFF).");
    }
}
