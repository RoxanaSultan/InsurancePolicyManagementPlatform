namespace InsuranceApp.Application.DTOs.Clients;

public class ClientIdentificationNumberAuditResponse
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string OldIdentificationNumber { get; set; } = default!;
    public string NewIdentificationNumber { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public DateTime ChangedAt { get; set; }
}