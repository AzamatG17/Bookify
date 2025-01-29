using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class FreeTimeRequestValidator : AbstractValidator<FreeTimeRequest>
{
    public FreeTimeRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .GreaterThan(0).WithMessage("ServiceId must be greater than 0.");

        RuleFor(x => x.DateOnly)
            .GreaterThan(DateTime.Now.Date.AddDays(-1)) 
            .WithMessage("Date must be in the future.");
    }
}
