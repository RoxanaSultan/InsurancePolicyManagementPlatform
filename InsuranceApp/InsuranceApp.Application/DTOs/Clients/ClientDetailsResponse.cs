using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Clients;

public class ClientDetailsResponse : ClientBase
{
    public int Id { get; set; }
    public ClientType ClientType { get; set; }
    public ICollection<BuildingResponse> Buildings { get; set; } = new List<BuildingResponse>();
}
