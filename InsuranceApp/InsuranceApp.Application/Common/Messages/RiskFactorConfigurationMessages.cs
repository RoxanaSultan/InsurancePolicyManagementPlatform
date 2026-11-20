using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Common.Messages;

public static class RiskFactorConfigurationMessages
{
    public static string NotFound(int riskFactorConfigurationId) =>
        $"RiskFactorConfiguration with id {riskFactorConfigurationId} not found.";

    public static string ActiveUniqueLevelAndReferenceExists(RiskFactorLevel level, int? referenceId) =>
        $"An active RiskFactorConfiguration already exists for {level} and {referenceId}.";

    public static string CountryNotFound(int countryId) =>
        $"Country with id {countryId} not found.";

    public static string CountyNotFound(int countyId) =>
        $"County with id {countyId} not found.";

    public static string CityNotFound(int cityId) =>
        $"City with id {cityId} not found.";

    public static string BuildingTypeNotFound(int buildingTypeId) =>
        $"BuildingType with id {buildingTypeId} not found.";

    public const string UnsupportedRiskFactorLevel =
        "Unsupported risk factor level.";
}