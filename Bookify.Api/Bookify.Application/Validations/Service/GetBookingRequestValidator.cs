using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class GetBookingRequestValidator : AbstractValidator<GetBookingRequest>
{
    public GetBookingRequestValidator()
    {
        RuleFor(b => b.BookingCode)
            .NotEmpty().WithMessage("Booking code is required.")
            .Length(6, 20).WithMessage("Booking code must be between 6 and 20 characters.");
    }
}
