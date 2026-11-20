using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Application.Services.Metadata;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class FeeConfigurationServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IFeeConfigurationRepository> FeeConfigurationRepository { get; } = new(MockBehavior.Strict);
    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public FeeConfigurationService Sut { get; }

    public FeeConfigurationServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.FeeConfigurations).Returns(FeeConfigurationRepository.Object);
        Sut = new FeeConfigurationService(UnitOfWork.Object, Mapper.Object, NullLogger<FeeConfigurationService>.Instance);
    }
}