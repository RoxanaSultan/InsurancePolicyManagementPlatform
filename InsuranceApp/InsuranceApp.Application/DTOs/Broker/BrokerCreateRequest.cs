using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Broker;

public class BrokerCreateRequest : BrokerBase
{
    public BrokerStatus Status { get; set; }
}