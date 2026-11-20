namespace InsuranceApp.Application.Common.Messages;

public static class FeeConfigurationMessages
{
    public static string NotFound(int feeConfigurationId) =>
        $"Fee configuration with id {feeConfigurationId} not found.";
}
