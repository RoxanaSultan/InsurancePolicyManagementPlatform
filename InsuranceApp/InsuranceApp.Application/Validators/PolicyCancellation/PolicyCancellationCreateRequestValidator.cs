using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.PolicyCancellation;

namespace InsuranceApp.Application.Validators.PolicyCancellation;

public class PolicyCancellationCreateRequestValidator : AbstractValidator<PolicyCancellationCreateRequest>
{
    public PolicyCancellationCreateRequestValidator()
    {
        RuleFor(x => x.CancellationDate)
            .NotEmpty().WithMessage(PolicyCancellationValidatorMessages.CancellationDateRequired)
            .Must(BeAValidDate).WithMessage(PolicyCancellationValidatorMessages.CancellationDateInvalid);

        RuleFor(x => x.CancellationReason)
            .NotEmpty().WithMessage(PolicyCancellationValidatorMessages.CancellationReasonRequired)
            .MaximumLength(500).WithMessage(PolicyCancellationValidatorMessages.CancellationReasonMaxLength);
    }

    private static bool BeAValidDate(DateTime date)
        => date != default;
}
