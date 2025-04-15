using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class UpdateServiceGroupRequestValidator : AbstractValidator<UpdateServiceGroupRequest>
{
    public UpdateServiceGroupRequestValidator()
    {
        RuleFor(x => x.ServiceGroupIds)
            .NotNull()
            .WithMessage("ServiceGroupIds are required.")
            .Must(x => x.Count > 0)
            .WithMessage("At least one group must be selected.");
    }
}
