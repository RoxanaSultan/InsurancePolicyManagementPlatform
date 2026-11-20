using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Register;
using System.Text.RegularExpressions;

namespace InsuranceApp.Application.Validators.Authentication;

public sealed partial class RegisterRequestValidator
    : AbstractValidator<RegisterRequest>
{
    private static readonly Regex SpecialCharacterRegex = SpecialCharacterRegexGenerated();

    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex SpecialCharacterRegexGenerated();

    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(RegisterValidatorMessages.EmailRequired)
            .MinimumLength(5).WithMessage(RegisterValidatorMessages.EmailMinLength)
            .MaximumLength(254).WithMessage(RegisterValidatorMessages.EmailMaxLength)
            .EmailAddress().WithMessage(RegisterValidatorMessages.EmailInvalid);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(RegisterValidatorMessages.PasswordRequired)
            .MinimumLength(8).WithMessage(RegisterValidatorMessages.PasswordMinLength)
            .Must(p => p.Any(char.IsLower)).WithMessage(RegisterValidatorMessages.PasswordLowercaseRequired)
            .Must(p => p.Any(char.IsUpper)).WithMessage(RegisterValidatorMessages.PasswordUppercaseRequired)
            .Must(p => p.Any(char.IsDigit)).WithMessage(RegisterValidatorMessages.PasswordDigitRequired)
            .Must(p => SpecialCharacterRegex.IsMatch(p)).WithMessage(RegisterValidatorMessages.PasswordSpecialCharacterRequired);
    }
}
