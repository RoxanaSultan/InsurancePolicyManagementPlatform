using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Currency;

namespace InsuranceApp.Application.Validators.Currency;

public class CurrencyUpdateRequestValidator : AbstractValidator<CurrencyUpdateRequest>
{
    public CurrencyUpdateRequestValidator()
    {
        Include(new CurrencyBaseValidator());

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage(CurrencyValidatorMessages.IsActiveRequired);
    }
}
