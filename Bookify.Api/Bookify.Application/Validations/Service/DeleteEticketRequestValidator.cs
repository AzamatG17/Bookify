using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class DeleteEticketRequestValidator : AbstractValidator<DeleteEticketRequest>
{
    public DeleteEticketRequestValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Ticket number is required.");

        RuleFor(x => x.SecondBranchId)
            .GreaterThan(0).WithMessage("Second branch ID must be greater than zero.");
    }
}
