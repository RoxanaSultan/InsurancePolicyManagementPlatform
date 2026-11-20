namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class BuildingValidatorMessages
{
    public const string AddressRequired =
        "Address is required.";

    public const string AddressStreetRequired =
        "Address street is required.";

    public const string AddressStreetMaxLength =
        "Address street cannot exceed 200 characters.";

    public const string AddressNumberRequired =
        "Address number is required.";

    public const string AddressNumberMaxLength =
        "Address number cannot exceed 20 characters.";

    public const string CityIdRequired =
        "City Id is required.";

    public static string ConstructionYearRange(int maxYear) =>
        $"Construction year must be between 1800 and {maxYear}.";

    public const string BuildingTypeInvalid =
        "Building type must be Residential, Industrial or Office.";

    public const string SurfaceAreaMin =
        "Surface area must be greater than 0.";

    public const string SurfaceAreaMax =
        "Surface area is unrealistically large.";

    public const string InsuredValueMin =
        "Insured value must be greater than 0.";

    public const string InsuredValueMax =
        "Insured value is unrealistically large.";

    public const string NumberOfFloorsMin =
        "Number of floors must be greater than 0";
}