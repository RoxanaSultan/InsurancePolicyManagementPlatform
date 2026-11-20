using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.RiskFactorConfiguration;

namespace InsuranceApp.Application.Validators.RiskFactorConfiguration;

public class RiskFactorConfigurationBaseValidator : AbstractValidator<RiskFactorConfigurationBase>
{
    public RiskFactorConfigurationBaseValidator()
    {
        RuleFor(x => x.Level)
            .IsInEnum().WithMessage(RiskFactorConfigurationValidatorMessages.LevelInvalid);

        RuleFor(x => x.ReferenceId)
            .NotEmpty().WithMessage(RiskFactorConfigurationValidatorMessages.ReferenceIdRequired);

        RuleFor(x => x.AdjustmentPercentage)
            .InclusiveBetween(-100, 100).WithMessage(RiskFactorConfigurationValidatorMessages.AdjustmentPercentageRange);
    }
}
