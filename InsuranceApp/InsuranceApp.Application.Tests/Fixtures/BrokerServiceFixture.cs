using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Broker;
using InsuranceApp.Application.Services.Broker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class BrokerServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IBrokerRepository> BrokerRepository { get; } = new(MockBehavior.Strict);
    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public BrokerService Sut { get; }

    public BrokerServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.Brokers).Returns(BrokerRepository.Object);
        Sut = new BrokerService(UnitOfWork.Object, Mapper.Object, NullLogger<BrokerService>.Instance);
    }
}