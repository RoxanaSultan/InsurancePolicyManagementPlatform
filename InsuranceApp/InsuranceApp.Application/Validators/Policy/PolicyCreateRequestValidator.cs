using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Policy;

namespace InsuranceApp.Application.Validators.Policy;

public class PolicyCreateRequestValidator : AbstractValidator<PolicyCreateRequest>
{
    public PolicyCreateRequestValidator()
    {
        Include(new PolicyBaseValidator());

        RuleFor(x => x.BuildingId)
            .GreaterThan(0).WithMessage(PolicyValidatorMessages.BuildingIdInvalid);

        RuleFor(x => x.CurrencyId)
            .GreaterThan(0).WithMessage(PolicyValidatorMessages.CurrencyIdInvalid);
    }
}
