using InsuranceApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace InsuranceApp.Application.DTOs.Clients;

public sealed class ClientUpdateRequest : ClientBase
{
    [JsonIgnore]
    public ClientType ClientType { get; set; }
}