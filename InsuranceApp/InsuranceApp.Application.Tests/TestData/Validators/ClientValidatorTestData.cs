using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.Tests.TestData;

public static class ClientValidatorTestData
{
    public static ClientBase ValidClientBase(
        string name = "John Doe",
        string identificationNumber = "1234567890123",
        string email = "test@test.com",
        string phone = "0712345678",
        Address? address = null)
        => new()
        {
            Name = name,
            IdentificationNumber = identificationNumber,
            ContactInfo = new ContactInfo(email, phone, address)
        };

    public static ClientCreateRequest ValidCreateRequest(
        string name = "John Doe",
        ClientType clientType = ClientType.Individual,
        string identificationNumber = "1234567890123",
        string email = "test@test.com",
        string phone = "0712345678",
        Address? address = null)
        => new()
        {
            Name = name,
            ClientType = clientType,
            IdentificationNumber = identificationNumber,
            ContactInfo = new ContactInfo(email, phone, address)
        };

    public static ClientUpdateRequest ValidUpdateRequest(
        string name = "ACME",
        ClientType clientType = ClientType.Company,
        string identificationNumber = "12",
        string email = "test@test.com",
        string phone = "0712345678",
        Address? address = null)
        => new()
        {
            Name = name,
            IdentificationNumber = identificationNumber,
            ContactInfo = new ContactInfo(email, phone, address),
            ClientType = clientType
        };

    public static ClientSearchRequest ValidSearchRequest(
        string? name = "John",
        string? identificationNumber = null)
        => new()
        {
            Name = name,
            IdentificationNumber = identificationNumber
        };
}
