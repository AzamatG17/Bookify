using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class GetBookingRequestValidator : AbstractValidator<GetBookingRequest>
{
    public GetBookingRequestValidator()
    {
        RuleFor(b => b.BookingCode)
            .NotEmpty().WithMessage("Booking code is required.")
            .Length(6, 15).WithMessage("Booking code must be between 6 and 15 characters.");

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
