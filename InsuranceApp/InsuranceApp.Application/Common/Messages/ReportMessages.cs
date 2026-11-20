using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Common.Messages;

public static class ReportMessages
{
    public static string PoliciesBy(ReportGroupField primaryGroup) =>
        $"Policies by {primaryGroup}";
}
