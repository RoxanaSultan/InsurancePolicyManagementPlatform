using System.Text.Json.Serialization;

namespace InsuranceApp.Application.DTOs.RiskFactorConfiguration;

public class RiskFactorConfigurationCreateRequest : RiskFactorConfigurationBase
{
    [JsonIgnore]
    public bool IsActive { get; set; } = true;
}