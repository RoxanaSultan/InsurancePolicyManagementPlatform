using FluentValidation;
using InsuranceApp.Application.DTOs.Buildings;

namespace InsuranceApp.Application.Validators.Buildings;

public class BuildingCreateRequestValidator : AbstractValidator<BuildingCreateRequest>
{
    public BuildingCreateRequestValidator()
    {
        Include(new BuildingBaseValidator());
    }
}
