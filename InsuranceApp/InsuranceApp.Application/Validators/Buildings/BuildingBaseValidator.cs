using FluentValidation;
using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Validators.Buildings;

public class BuildingBaseValidator : AbstractValidator<BuildingBase>
{
    public BuildingBaseValidator()
    {
        // Address
        RuleFor(x => x.Address)
            .NotNull().WithMessage(BuildingValidatorMessages.AddressRequired);

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address!.Street)
                .NotEmpty().WithMessage(BuildingValidatorMessages.AddressStreetRequired)
                .MaximumLength(200).WithMessage(BuildingValidatorMessages.AddressStreetMaxLength);

            RuleFor(x => x.Address!.Number)
                .NotEmpty().WithMessage(BuildingValidatorMessages.AddressNumberRequired)
                .MaximumLength(20).WithMessage(BuildingValidatorMessages.AddressNumberMaxLength);
        });

        // City
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessage(BuildingValidatorMessages.CityIdRequired);

        // Construction year
        var maxYear = DateTime.UtcNow.Year;
        RuleFor(x => x.ConstructionYear)
            .InclusiveBetween(1800, maxYear)
            .WithMessage(BuildingValidatorMessages.ConstructionYearRange(maxYear));

        // Building type (enum restricted)
        RuleFor(x => x.BuildingType)
            .IsInEnum()
            .Must(bt =>
                bt == BuildingType.Residential ||
                bt == BuildingType.Industrial ||
                bt == BuildingType.Office)
            .WithMessage(BuildingValidatorMessages.BuildingTypeInvalid);

        // Surface area
        RuleFor(x => x.SurfaceArea)
            .GreaterThan(0).WithMessage(BuildingValidatorMessages.SurfaceAreaMin)
            .LessThanOrEqualTo(1_000_000).WithMessage(BuildingValidatorMessages.SurfaceAreaMax);

        // Insured value
        RuleFor(x => x.InsuredValue)
            .GreaterThan(0).WithMessage(BuildingValidatorMessages.InsuredValueMin)
            .LessThanOrEqualTo(1_000_000_000).WithMessage(BuildingValidatorMessages.InsuredValueMax);

        // Number of floors
        RuleFor(x => x.NumberOfFloors)
            .GreaterThan(0).WithMessage(BuildingValidatorMessages.NumberOfFloorsMin);
    }
}
