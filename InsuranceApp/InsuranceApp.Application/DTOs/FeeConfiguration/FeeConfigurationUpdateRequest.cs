using InsuranceApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace InsuranceApp.Application.DTOs.FeeConfiguration;

public class FeeConfigurationUpdateRequest : FeeConfigurationBase
{
    public bool IsActive { get; set; }
}