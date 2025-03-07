using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class FreeTimeRequestValidator : AbstractValidator<FreeTimeRequest>
{
    public FreeTimeRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .GreaterThan(0).WithMessage("ID услуги должен быть больше 0.");

        RuleFor(x => x.DateOnly)
            .GreaterThan(DateTime.Now.Date.AddDays(-1))
            .WithMessage("Дата должна быть в будущем.");
    }
}
