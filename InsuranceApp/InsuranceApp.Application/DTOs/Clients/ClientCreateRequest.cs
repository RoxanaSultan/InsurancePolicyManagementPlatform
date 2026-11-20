using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.DTOs.Clients;

public sealed class ClientCreateRequest : ClientBase
{
    public ClientType ClientType { get; set; }
}