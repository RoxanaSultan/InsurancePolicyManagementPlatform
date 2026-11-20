using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.TestData;

public static class BrokerValidatorTestData
{
    public static BrokerBase ValidBrokerBase(
        string name = "John Doe",
        string email = "broker@test.com",
        string phone = "0712345678",
        decimal? commission = 10m)
        => new()
        {
            Name = name,
            Email = email,
            Phone = phone,
            CommissionPercentage = commission
        };

    public static BrokerCreateRequest ValidCreateRequest(
        BrokerStatus status = BrokerStatus.Active)
        => new()
        {
            Name = "John Doe",
            Email = "broker@test.com",
            Phone = "0712345678",
            Status = status,
            CommissionPercentage = 10m
        };

    public static BrokerUpdateRequest ValidUpdateRequest()
        => new()
        {
            Name = "John Doe",
            Email = "broker@test.com",
            Phone = "0712345678",
            CommissionPercentage = 10m
        };
}
