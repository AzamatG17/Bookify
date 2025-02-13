using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class EticketStatusRequestValidator : AbstractValidator<EticketStatusRequest>
{
    public EticketStatusRequestValidator()
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0).WithMessage("TicketId must be greater than 0");

        RuleFor(x => x.BranchId)
            .GreaterThan(0).WithMessage("BranchId must be greater than 0");
    }
}
