using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class ServiceGroupRequestValidator : AbstractValidator<ServiceGroupRequest>
{
    public ServiceGroupRequestValidator()
    {
        RuleFor(x => x.ServiceGroupTranslationDtos)
            .NotNull()
            .WithMessage("Translations are required.")
            .NotEmpty()
            .WithMessage("At least one translation is required.");

        RuleForEach(x => x.ServiceGroupTranslationDtos).SetValidator(new ServiceGroupTranslationDtoValidator());
    }
}
