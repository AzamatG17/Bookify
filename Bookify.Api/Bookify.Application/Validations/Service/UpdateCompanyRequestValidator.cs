using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.BaseUrl)
            .NotEmpty()
            .WithMessage("BaseUrl is required.");

        RuleFor(x => x.Projects)
            .NotNull()
            .WithMessage("Projects cannot be null.");

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
