using InsuranceApp.Domain.Common;
using InsuranceApp.Domain.Enums;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Domain.Entities.User;

public class Broker : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public BrokerStatus Status { get; set; }
    public decimal? CommissionPercentage { get; set; }

    public string? UserId { get; set; }

    public ICollection<PolicyEntity> Policies { get; set; } = [];
}