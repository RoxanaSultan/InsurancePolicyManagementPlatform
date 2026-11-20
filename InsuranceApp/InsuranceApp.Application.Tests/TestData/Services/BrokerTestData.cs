using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class BrokerTestData
{
    public static BrokerCreateRequest ValidCreateRequest(
        string email = "a@broker.com",
        string phone = "0712345678")
        => new()
        {
            Name = "Broker One",
            Email = email,
            Phone = phone,
            Status = BrokerStatus.Active,
            CommissionPercentage = 10m
        };

    public static BrokerUpdateRequest ValidUpdateRequest(
        string email = "a@broker.com",
        string phone = "0712345678")
        => new()
        {
            Name = "Broker Two",
            Email = email,
            Phone = phone,
            CommissionPercentage = 12m
        };

    public static Broker BrokerEntity(
        int id = 42,
        BrokerStatus status = BrokerStatus.Inactive)
        => new()
        {
            Id = id,
            Status = status
        };
}