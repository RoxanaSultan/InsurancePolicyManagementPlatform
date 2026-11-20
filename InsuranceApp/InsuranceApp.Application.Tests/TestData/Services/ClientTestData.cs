using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class ClientTestData
{
    public static ClientCreateRequest ValidCreateRequest(
        string id = "CNP1",
        string email = "a@b.com",
        string phone = "0711")
        => new()
        {
            Name = "Ion Popescu",
            IdentificationNumber = id,
            ClientType = ClientType.Individual,
            ContactInfo = new ContactInfo
            {
                Email = email,
                PhoneNumber = phone,
                Address = new Address
                {
                    Street = "Libertatii",
                    Number = "10"
                }
            }
        };

    public static ClientUpdateRequest ValidUpdateRequest(
        string id = "NEW",
        string email = "a@b.com",
        string phone = "0711")
        => new()
        {
            Name = "Nume Nou",
            IdentificationNumber = id,
            ContactInfo = new ContactInfo
            {
                Email = email,
                PhoneNumber = phone,
                Address = new Address
                {
                    Street = "Memorandului",
                    Number = "15"
                }
            }
        };
}