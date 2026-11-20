using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Domain.Enums;
using System.Text.RegularExpressions;

namespace InsuranceApp.Application.Validators.Clients;

public sealed partial class ClientCreateRequestValidator
    : AbstractValidator<ClientCreateRequest>
{
    private static readonly Regex IndividualCnpRegex = IndividualCnpRegexGenerated();
    private static readonly Regex CompanyRegistrationRegex = CompanyRegistrationRegexGenerated();

    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex IndividualCnpRegexGenerated();

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex CompanyRegistrationRegexGenerated();

    public ClientCreateRequestValidator()
    {
        Include(new ClientBaseValidator());

        RuleFor(x => x.ClientType)
            .NotNull().WithMessage(ClientValidatorMessages.ClientTypeRequired)
            .Must(ct => ct == ClientType.Individual || ct == ClientType.Company)
            .WithMessage(ClientValidatorMessages.ClientTypeInvalid);

        RuleFor(x => x.IdentificationNumber)
            .NotEmpty().WithMessage(ClientValidatorMessages.IdentificationNumberRequired);

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
