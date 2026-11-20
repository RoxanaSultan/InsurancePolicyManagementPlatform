using FluentValidation;
using InsuranceApp.Application.DTOs.Broker;

namespace InsuranceApp.Application.Validators.Broker;

public class BrokerUpdateRequestValidator : AbstractValidator<BrokerUpdateRequest>
{
    public BrokerUpdateRequestValidator()
    {
        Include(new BrokerBaseValidator());
    }
}