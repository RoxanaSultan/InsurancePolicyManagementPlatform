namespace InsuranceApp.Domain.ValueObjects;

public sealed record Address
{
    public string Street { get; init; } = null!;
    public string Number { get; init; } = null!;

    public Address() { }

    public Address(string street, string number)
        => (Street, Number) = (street, number);
}