using InsuranceApp.Domain.Common;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;
using ClientEntity = InsuranceApp.Domain.Entities.Client.Client;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Domain.Entities.Building;

public class Building : BaseEntity
{
    public Address Address { get; set; } = new(null!, null!);
    public int ConstructionYear { get; set; }
    public int NumberOfFloors { get; set; }
    public decimal SurfaceArea { get; set; }
    public decimal InsuredValue { get; set; }
    public BuildingType BuildingType { get; set; }

    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;

    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public ICollection<PolicyEntity> Policies { get; set; } = [];
}