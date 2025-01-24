using Bookify.Application.Requests.Services;
using FluentValidation;

namespace Bookify.Application.Validations.Service;

public class CompaniesRequestValidator : AbstractValidator<CompanyRequest>
{
    public CompaniesRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");
    }
}
