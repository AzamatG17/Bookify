using Bookify.Application.Requests.Services;
using FluentValidation;
using System.Globalization;

namespace Bookify.Application.Validations.Service;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .GreaterThan(0).WithMessage("ServiceId должен быть больше 0.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.Now.Date.AddDays(-1))
            .WithMessage("Дата должна быть в будущем.");

        RuleFor(x => x.StartTime)
            .Must(BeValidTimeFormat).WithMessage("StartTime должен быть в формате HH:mm:ss.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Язык обязателен.")
            .Must(IsValidLanguage).WithMessage("Язык должен быть валидным ISO-кодом (например, 'en', 'ru', 'uz').");
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
