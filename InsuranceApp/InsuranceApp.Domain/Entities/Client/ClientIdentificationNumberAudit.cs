using InsuranceApp.Domain.Common;

namespace InsuranceApp.Domain.Entities.Client;

public class ClientIdentificationNumberAudit : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string OldIdentificationNumber { get; set; } = string.Empty;
    public string NewIdentificationNumber { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}