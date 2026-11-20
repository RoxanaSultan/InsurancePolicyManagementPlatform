namespace InsuranceApp.Application.Common.Messages;

public static class PolicyMessages
{
    public static string NotFoundWithID(int policyId) =>
        $"Policy with ID {policyId} not found.";

    public static string NotFoundWithId(int policyId) =>
        $"Policy with Id {policyId} not found.";

    public static string OnlyActivePoliciesCanBeCancelled(string status) =>
        $"Only active policies can be cancelled. Current status: {status}.";

    public static string BrokerForUserNotFound(string? userId) =>
        $"Broker for UserId {userId} not found.";

    public static string BuildingNotFound(int buildingId) =>
        $"Building with Id {buildingId} not found.";

    public static string CityNotFound(int cityId) =>
        $"City with Id {cityId} not found.";

    public static string CountyNotFound(int countyId) =>
        $"County with Id {countyId} not found.";

    public static string CountryNotFound(int countryId) =>
        $"Country with Id {countryId} not found.";
}
