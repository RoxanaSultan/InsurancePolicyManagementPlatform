using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Domain.Enums;
using System.Text.RegularExpressions;

namespace InsuranceApp.Application.Validators.Clients;

public sealed partial class ClientUpdateRequestValidator : AbstractValidator<ClientUpdateRequest>
{
    private static readonly Regex IndividualCnpRegex = IndividualCnpRegexGenerated();
    private static readonly Regex CompanyRegistrationRegex = CompanyRegistrationRegexGenerated();

    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex IndividualCnpRegexGenerated();

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex CompanyRegistrationRegexGenerated();

    public ClientUpdateRequestValidator()
    {
        Include(new ClientBaseValidator());

        When(x => x.ClientType == ClientType.Individual, () =>
        {
            RuleFor(x => x.IdentificationNumber)
                .Must(id => IndividualCnpRegex.IsMatch(id ?? string.Empty))
                .WithMessage(ClientValidatorMessages.CnpInvalid);
        });

        When(x => x.ClientType == ClientType.Company, () =>
        {
            RuleFor(x => x.IdentificationNumber)
                .Must(id => CompanyRegistrationRegex.IsMatch(id ?? string.Empty))
                .WithMessage(ClientValidatorMessages.CompanyRegistrationDigitsOnly)
                .MinimumLength(2).WithMessage(ClientValidatorMessages.CompanyRegistrationMinLength)
                .MaximumLength(20).WithMessage(ClientValidatorMessages.CompanyRegistrationMaxLength);
        });
    }
}
