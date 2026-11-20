using InsuranceApp.Domain.Common;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;

namespace InsuranceApp.Domain.Entities.Geography;

public class City : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int CountyId { get; set; }
    public County County { get; set; } = null!;

    public ICollection<BuildingEntity> Buildings { get; set; } = [];
}