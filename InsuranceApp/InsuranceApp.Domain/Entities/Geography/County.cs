using InsuranceApp.Domain.Common;

namespace InsuranceApp.Domain.Entities.Geography;

public class County : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = null!;

    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;

    public ICollection<City> Cities { get; set; } = [];
}