using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class ServiceRequestValidator : AbstractValidator<ServiceRequest>
{
    public ServiceRequestValidator()
    {
        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .WithMessage("BranchId must be greater than 0.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.")
            .Must(IsValidLanguage).WithMessage("Language must be a valid ISO code (e.g., 'en', 'ru', 'uz').");
    }

    private bool IsValidLanguage(string language)
    {
        var validLanguages = new[] { "en", "ru", "uz" };
        return validLanguages.Contains(language.ToLower());
    }
}
