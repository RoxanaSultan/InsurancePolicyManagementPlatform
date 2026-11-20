using FluentValidation;
using InsuranceApp.Application.DTOs.Currency;

namespace InsuranceApp.Application.Validators.Currency;

public class CurrencyCreateRequestValidator : AbstractValidator<CurrencyCreateRequest>
{
    public CurrencyCreateRequestValidator()
    {
        Include(new CurrencyBaseValidator());
    }
}