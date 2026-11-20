using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Validators.Clients;

public class PageRequestValidator : AbstractValidator<PageRequest>
{
    public PageRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage(PageRequestValidatorMessages.PageMin);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage(PageRequestValidatorMessages.PageSizeRange);
    }
}
