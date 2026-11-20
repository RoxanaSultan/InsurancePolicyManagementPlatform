using FluentValidation;
using InsuranceApp.Application.DTOs.FeeConfiguration;

namespace InsuranceApp.Application.Validators.FeeConfiguration;

public class FeeConfigurationCreateRequestValidator : AbstractValidator<FeeConfigurationCreateRequest>
{
    public FeeConfigurationCreateRequestValidator()
    {
        Include(new FeeConfigurationBaseValidator());
    }
}