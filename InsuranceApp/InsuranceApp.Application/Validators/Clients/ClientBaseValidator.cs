using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Clients;

namespace InsuranceApp.Application.Validators.Clients;

public class ClientBaseValidator : AbstractValidator<ClientBase>
{
    public ClientBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ClientValidatorMessages.NameRequired)
            .MinimumLength(2).WithMessage(ClientValidatorMessages.NameMinLength)
            .MaximumLength(100).WithMessage(ClientValidatorMessages.NameMaxLength);

        RuleFor(x => x.ContactInfo)
            .NotNull().WithMessage(ClientValidatorMessages.ContactInfoRequired);

        When(x => x.ContactInfo is not null, () =>
        {
            RuleFor(x => x.ContactInfo!.Email)
                .NotEmpty().WithMessage(ClientValidatorMessages.EmailRequired)
                .MinimumLength(5).WithMessage(ClientValidatorMessages.EmailMinLength)
                .MaximumLength(254).WithMessage(ClientValidatorMessages.EmailMaxLength)
                .EmailAddress().WithMessage(ClientValidatorMessages.EmailInvalid);

            RuleFor(x => x.ContactInfo!.PhoneNumber)
                .NotEmpty().WithMessage(ClientValidatorMessages.PhoneRequired)
                .Matches(@"^0\d{9}$").WithMessage(ClientValidatorMessages.PhoneInvalid);

            When(x => x.ContactInfo!.Address is not null, () =>
            {
                RuleFor(x => x.ContactInfo!.Address!.Street)
                    .NotEmpty().WithMessage(ClientValidatorMessages.StreetRequired)
                    .MinimumLength(2).WithMessage(ClientValidatorMessages.StreetMinLength)
                    .MaximumLength(200).WithMessage(ClientValidatorMessages.StreetMaxLength);

                RuleFor(x => x.ContactInfo!.Address!.Number)
                    .NotEmpty().WithMessage(ClientValidatorMessages.NumberRequired)
                    .MinimumLength(1).WithMessage(ClientValidatorMessages.NumberMinLength)
                    .MaximumLength(20).WithMessage(ClientValidatorMessages.NumberMaxLength);
            });
        });
    }
}