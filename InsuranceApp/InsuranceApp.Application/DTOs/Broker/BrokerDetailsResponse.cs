using InsuranceApp.Domain.Enums;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Application.DTOs.Broker;

public class BrokerDetailsResponse : BrokerBase
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public BrokerStatus Status { get; set; }
    public string UserId { get; set; } = null!;
    public ICollection<PolicyEntity> Policies { get; set; } = new List<PolicyEntity>();
}