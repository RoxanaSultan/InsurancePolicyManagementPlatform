namespace InsuranceApp.Application.DTOs.Clients;

public sealed class ClientSearchRequest
{
    public string? Name { get; set; }
    public string? IdentificationNumber { get; set; }
}