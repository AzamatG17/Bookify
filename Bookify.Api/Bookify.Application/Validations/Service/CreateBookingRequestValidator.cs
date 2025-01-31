using Bookify.Application.Requests.Services;
using FluentValidation;
using System.Globalization;

namespace Bookify.Application.Validations.Service;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .GreaterThan(0).WithMessage("ServiceId must be greater than 0.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.Now.Date.AddDays(-1))
            .WithMessage("Date must be in the future.");

        RuleFor(x => x.StartTime)
            .Must(BeValidTimeFormat).WithMessage("StartTime must be in HH:mm:ss format.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.")
            .Must(IsValidLanguage).WithMessage("Language must be a valid ISO code (e.g., 'en', 'ru', 'uz').");
    }

    private bool IsValidLanguage(string language)
    {
        var validLanguages = new[] { "en", "ru", "uz" };
        return validLanguages.Contains(language.ToLower());
    }

    private bool BeValidTimeFormat(string time)
    {
        return TimeSpan.TryParseExact(time, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out _);
    }
}
