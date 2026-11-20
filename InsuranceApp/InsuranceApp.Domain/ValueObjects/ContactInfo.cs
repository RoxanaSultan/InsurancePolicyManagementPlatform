using System.Net;

namespace InsuranceApp.Domain.ValueObjects;

public record ContactInfo
{
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public Address? Address { get; init; }

    public ContactInfo()
        : this(string.Empty, string.Empty, null) { }

    public ContactInfo(string email, string phoneNumber, Address? address)
        => (Email, PhoneNumber, Address) = (email, phoneNumber, address);
}