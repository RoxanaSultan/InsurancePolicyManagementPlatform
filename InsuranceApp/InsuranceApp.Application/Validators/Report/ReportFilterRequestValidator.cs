using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Report;

namespace InsuranceApp.Application.Validators.Report;

public class ReportFilterRequestValidator : AbstractValidator<ReportFilterRequest>
{
    public ReportFilterRequestValidator()
    {
        RuleFor(x => x.From)
            .NotEqual(default(DateTime))
            .WithMessage(ReportValidatorMessages.FromRequired);

        RuleFor(x => x.To)
            .NotEqual(default(DateTime))
            .WithMessage(ReportValidatorMessages.ToRequired);

        RuleFor(x => x)
            .Must(x => x.To >= x.From)
            .WithMessage(ReportValidatorMessages.ToAfterFrom);

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue);

        RuleFor(x => x.BuildingType)
            .IsInEnum()
            .When(x => x.BuildingType.HasValue);
    }
}
