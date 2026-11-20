using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.RiskFactorConfiguration;

namespace InsuranceApp.Application.Validators.RiskFactorConfiguration;

public class RiskFactorConfigurationUpdateRequestValidator : AbstractValidator<RiskFactorConfigurationUpdateRequest>
{
    public RiskFactorConfigurationUpdateRequestValidator()
    {
        Include(new RiskFactorConfigurationBaseValidator());

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage(RiskFactorConfigurationValidatorMessages.IsActiveRequired);
    }
}
