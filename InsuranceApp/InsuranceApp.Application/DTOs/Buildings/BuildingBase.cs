using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.DTOs.Buildings;

public class BuildingBase
{
    public Address Address { get; set; } = new(null!, null!);
    public int CityId { get; set; }
    public int ConstructionYear { get; set; }
    public BuildingType BuildingType { get; set; }
    public decimal SurfaceArea { get; set; }
    public decimal InsuredValue { get; set; }
    public int NumberOfFloors { get; set; }
}
