using FluentValidation;
using InsuranceApp.Application.DTOs.RiskFactorConfiguration;

namespace InsuranceApp.Application.Validators.RiskFactorConfiguration;

public class RiskFactorConfigurationCreateRequestValidator : AbstractValidator<RiskFactorConfigurationCreateRequest>
{
    public RiskFactorConfigurationCreateRequestValidator()
    {
        Include(new RiskFactorConfigurationBaseValidator());
    }
}