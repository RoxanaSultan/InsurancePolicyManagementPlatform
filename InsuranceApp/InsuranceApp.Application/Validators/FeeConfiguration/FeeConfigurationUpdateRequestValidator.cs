using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.FeeConfiguration;

namespace InsuranceApp.Application.Validators.FeeConfiguration;

public class FeeConfigurationUpdateRequestValidator : AbstractValidator<FeeConfigurationUpdateRequest>
{
    public FeeConfigurationUpdateRequestValidator()
    {
        Include(new FeeConfigurationBaseValidator());

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage(FeeConfigurationValidatorMessages.IsActiveRequired);
    }
}
