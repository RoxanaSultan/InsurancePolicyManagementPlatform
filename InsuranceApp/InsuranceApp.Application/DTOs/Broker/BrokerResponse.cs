using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Broker;

public class BrokerResponse : BrokerBase
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public BrokerStatus Status { get; set; }
    public string UserId { get; set; } = null!;
}