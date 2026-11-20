using System.Text.Json.Serialization;

namespace InsuranceApp.Application.DTOs.PolicyCancellation;

public class PolicyCancellationCreateRequest
{
    public DateTime CancellationDate { get; set; }
    public string CancellationReason { get; set; } = null!;

    [JsonIgnore]
    public int PolicyId { get; set; }
}