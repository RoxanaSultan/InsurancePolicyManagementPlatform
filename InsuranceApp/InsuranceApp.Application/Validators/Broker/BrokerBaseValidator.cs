using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Broker;

namespace InsuranceApp.Application.Validators.Broker;

public class BrokerBaseValidator : AbstractValidator<BrokerBase>
{
    public BrokerBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(BrokerValidatorMessages.NameRequired)
            .MaximumLength(100).WithMessage(BrokerValidatorMessages.NameMaxLength)
            .Matches(@"^[A-Za-z]+([ '\-][A-Za-z]+)*$").WithMessage(BrokerValidatorMessages.NameInvalidCharacters);

        RuleFor(x => x.Email)
                .NotEmpty().WithMessage(BrokerValidatorMessages.EmailRequired)
                .MinimumLength(5).WithMessage(BrokerValidatorMessages.EmailMinLength)
                .MaximumLength(254).WithMessage(BrokerValidatorMessages.EmailMaxLength)
                .EmailAddress().WithMessage(BrokerValidatorMessages.EmailInvalid);
 
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(BrokerValidatorMessages.PhoneRequired)
            .Matches(@"^0\d{9}$").WithMessage(BrokerValidatorMessages.PhoneInvalid);

        RuleFor(x => x.CommissionPercentage)
            .GreaterThanOrEqualTo(0).WithMessage(BrokerValidatorMessages.CommissionPercentageMin);
    }
}
