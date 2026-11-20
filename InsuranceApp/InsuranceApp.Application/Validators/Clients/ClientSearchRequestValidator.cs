using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Clients;

namespace InsuranceApp.Application.Validators.Clients;

public class ClientSearchRequestValidator : AbstractValidator<ClientSearchRequest>
{
    public ClientSearchRequestValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.IdentificationNumber), () =>
        {
            RuleFor(x => x.IdentificationNumber!)
                .MinimumLength(5);
        });

        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name!)
                .MinimumLength(2);
        });
    }
}
