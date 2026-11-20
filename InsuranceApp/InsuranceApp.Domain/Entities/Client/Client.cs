using InsuranceApp.Domain.Common;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;

namespace InsuranceApp.Domain.Entities.Client;

public class Client : BaseEntity
{
    public ClientType ClientType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public ContactInfo ContactInfo { get; set; } = new(null!, null!, null);

    public ICollection<BuildingEntity> Buildings { get; set; } = [];
}