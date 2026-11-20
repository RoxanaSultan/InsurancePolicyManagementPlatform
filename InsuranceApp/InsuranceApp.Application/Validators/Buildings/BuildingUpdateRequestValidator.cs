using FluentValidation;
using InsuranceApp.Application.DTOs.Buildings;

namespace InsuranceApp.Application.Validators.Buildings;

public class BuildingUpdateRequestValidator : AbstractValidator<BuildingUpdateRequest>
{
    public BuildingUpdateRequestValidator()
    {
        Include(new BuildingBaseValidator());
    }
}