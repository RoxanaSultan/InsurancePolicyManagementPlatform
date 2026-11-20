using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.FeeConfiguration;

namespace InsuranceApp.Application.Validators.FeeConfiguration;

public class FeeConfigurationBaseValidator : AbstractValidator<FeeConfigurationBase>
{
    public FeeConfigurationBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(FeeConfigurationValidatorMessages.NameRequired)
            .MaximumLength(150).WithMessage(FeeConfigurationValidatorMessages.NameMaxLength);

        RuleFor(x => x.Type)
            .NotNull().WithMessage(FeeConfigurationValidatorMessages.TypeRequired)
            .IsInEnum().WithMessage(FeeConfigurationValidatorMessages.TypeInvalid);

        RuleFor(x => x.Percentage)
            .NotNull().WithMessage(FeeConfigurationValidatorMessages.PercentageRequired)
            .InclusiveBetween(0.01m, 100.00m).WithMessage(FeeConfigurationValidatorMessages.PercentageRange);

        RuleFor(x => x.EffectiveFrom)
            .NotNull().WithMessage(FeeConfigurationValidatorMessages.EffectiveFromRequired);

        RuleFor(x => x.EffectiveTo)
            .GreaterThan(x => x.EffectiveFrom).WithMessage(FeeConfigurationValidatorMessages.EffectiveToAfterFrom);
    }
}
