using InsuranceApp.Domain.Common;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Domain.Entities.User;

public class Administrator : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public AdminRole Role { get; set; }

    public string UserId { get; set; } = null!;
}