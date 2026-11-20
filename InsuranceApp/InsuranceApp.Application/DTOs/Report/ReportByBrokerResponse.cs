using InsuranceApp.Application.DTOs.Broker;

namespace InsuranceApp.Application.DTOs.Report;

public class ReportByBrokerResponse : ReportResponse
{
    public required BrokerResponse Broker { get; set; }
}
