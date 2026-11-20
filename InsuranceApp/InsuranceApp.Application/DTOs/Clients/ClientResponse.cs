using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Clients;

public class ClientResponse : ClientBase
{
    public int Id { get; set; }
    public ClientType ClientType { get; set; }
}
