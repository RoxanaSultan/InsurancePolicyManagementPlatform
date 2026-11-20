using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Currency;

namespace InsuranceApp.Application.Validators.Currency;

public class CurrencyBaseValidator : AbstractValidator<CurrencyBase>
{
    public CurrencyBaseValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(CurrencyValidatorMessages.CodeRequired)
            .Length(3).WithMessage(CurrencyValidatorMessages.CodeLength)
            .Matches("^[A-Za-z]{3}$").WithMessage(CurrencyValidatorMessages.CodeLettersOnly);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CurrencyValidatorMessages.NameRequired)
            .MaximumLength(100).WithMessage(CurrencyValidatorMessages.NameMaxLength)
            .Matches(@"^[A-Za-z]+([ '\-][A-Za-z]+)*$").WithMessage(CurrencyValidatorMessages.NameInvalidCharacters);

        RuleFor(x => x.ExchangeRateToBase)
            .NotNull().WithMessage(CurrencyValidatorMessages.ExchangeRateRequired)
            .GreaterThan(0).WithMessage(CurrencyValidatorMessages.ExchangeRateMin);
    }
}
