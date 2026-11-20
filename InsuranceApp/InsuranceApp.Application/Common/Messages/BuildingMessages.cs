namespace InsuranceApp.Application.Common.Messages;

public static class BuildingMessages
{
    public static string ClientNotFound(int clientId) =>
        $"Client with id {clientId} not found.";

    public static string CityNotFound(int cityId) =>
        $"City with id {cityId} not found.";

    public static string BuildingNotFound(int buildingId) =>
        $"Building with id {buildingId} not found.";
}
