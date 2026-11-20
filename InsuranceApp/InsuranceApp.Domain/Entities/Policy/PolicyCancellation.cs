using InsuranceApp.Domain.Common;

namespace InsuranceApp.Domain.Entities.Policy;

public class PolicyCancellation : BaseEntity
{
    public DateTime CancellationDate { get; set; }
    public string CancellationReason { get; set; } = null!;

    public int PolicyId { get; set; }
}