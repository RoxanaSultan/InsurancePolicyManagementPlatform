using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Policy;

namespace InsuranceApp.Application.Validators.Policy;

public class PolicyBaseValidator : AbstractValidator<PolicyBase>
{
    public PolicyBaseValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage(PolicyValidatorMessages.StartDateRequired)
            .Must(BeAValidDate).WithMessage(PolicyValidatorMessages.StartDateInvalid);

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage(PolicyValidatorMessages.EndDateRequired)
            .Must(BeAValidDate).WithMessage(PolicyValidatorMessages.EndDateInvalid);

        RuleFor(x => x)
            .Must(x => x.EndDate > x.StartDate).WithMessage(PolicyValidatorMessages.EndDateAfterStart);

        RuleFor(x => x.BasePremiumAmount)
            .GreaterThan(0m).WithMessage(PolicyValidatorMessages.BasePremiumMin)
            .Must(HaveMaxTwoDecimals).WithMessage(PolicyValidatorMessages.BasePremiumMaxDecimals)
            .LessThanOrEqualTo(9999999999999999.99m).WithMessage(PolicyValidatorMessages.BasePremiumTooLarge);
    }

    private static bool BeAValidDate(DateTime date)
        => date != default;

    private static bool HaveMaxTwoDecimals(decimal value)
        => decimal.Round(value, 2) == value;
}
