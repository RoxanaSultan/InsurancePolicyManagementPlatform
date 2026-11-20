using InsuranceApp.Domain.Common;

namespace InsuranceApp.Domain.Entities.Geography;

public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<County> Counties { get; set; } = [];
}