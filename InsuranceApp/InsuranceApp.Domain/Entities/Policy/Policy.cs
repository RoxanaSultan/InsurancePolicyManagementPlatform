using InsuranceApp.Domain.Common;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Domain.Enums;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;

namespace InsuranceApp.Domain.Entities.Policy;

public class Policy : BaseEntity
{
    public PolicyStatus Status { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal BasePremiumAmount { get; set; }
    public decimal CalculatedPremiumAmount { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }

    public int BuildingId { get; set; }
    public BuildingEntity Building { get; set; } = null!;

    public int BrokerId { get; set; }
    public Broker Broker { get; set; } = null!;

    public int CurrencyId { get; set; }
    public Currency Currency { get; set; } = null!;

    public void Activate()
    {
        Status = PolicyStatus.Active;
        LastUpdateDate = DateTime.UtcNow;
    }

    public void Draft()
    {
        Status = PolicyStatus.Draft;
        LastUpdateDate = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = PolicyStatus.Cancelled;
        LastUpdateDate = DateTime.UtcNow;
    }

    public void Expire()
    {
        Status = PolicyStatus.Expired;
        LastUpdateDate = DateTime.UtcNow;
    }
} 