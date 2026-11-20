using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Broker;

namespace InsuranceApp.Application.Validators.Broker;

public class BrokerCreateRequestValidator : AbstractValidator<BrokerCreateRequest>
{
    public BrokerCreateRequestValidator()
    {
        Include(new BrokerBaseValidator());

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage(BrokerValidatorMessages.StatusInvalid);
    }
}
