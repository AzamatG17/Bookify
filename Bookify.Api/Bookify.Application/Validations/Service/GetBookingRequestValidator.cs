using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class GetBookingRequestValidator : AbstractValidator<GetBookingRequest>
{
    public GetBookingRequestValidator()
    {
        RuleFor(b => b.BookingCode)
            .NotEmpty().WithMessage("Код бронирования обязателен для заполнения.")
            .Length(6, 15).WithMessage("Код бронирования должен содержать от 6 до 15 символов.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Язык обязателен для заполнения.")
            .Must(IsValidLanguage).WithMessage("Язык должен быть допустимым ISO-кодом (например, 'en', 'ru', 'uz').");
    }

    private bool IsValidLanguage(string language)
    {
        var validLanguages = new[] { "en", "ru", "uz" };
        return validLanguages.Contains(language.ToLower());
    }
}
