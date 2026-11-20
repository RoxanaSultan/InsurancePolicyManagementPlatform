using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.DTOs.Clients;

public class ClientBase
{
    public string Name { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public ContactInfo ContactInfo { get; set; } = new(null!, null!, null);
}