using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Policy;

public class PolicyFilterRequest
{
    public int? ClientId { get; set; }
    public int? BrokerId { get; set; }
    public PolicyStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}