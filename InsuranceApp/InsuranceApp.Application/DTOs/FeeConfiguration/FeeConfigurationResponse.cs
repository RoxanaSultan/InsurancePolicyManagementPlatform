using InsuranceApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace InsuranceApp.Application.DTOs.FeeConfiguration;

public class FeeConfigurationResponse : FeeConfigurationBase
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
}