using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Application.Services.Metadata;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class RiskFactorConfigurationServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IRiskFactorConfigurationRepository> RiskFactorRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICountryRepository> CountryRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICountyRepository> CountyRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICityRepository> CityRepository { get; } = new(MockBehavior.Strict);
    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public RiskFactorConfigurationService Sut { get; }

    public RiskFactorConfigurationServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.RiskFactorConfigurations).Returns(RiskFactorRepository.Object);
        UnitOfWork.SetupGet(u => u.Countries).Returns(CountryRepository.Object);
        UnitOfWork.SetupGet(u => u.Counties).Returns(CountyRepository.Object);
        UnitOfWork.SetupGet(u => u.Cities).Returns(CityRepository.Object);

        Sut = new RiskFactorConfigurationService(UnitOfWork.Object, Mapper.Object, NullLogger<RiskFactorConfigurationService>.Instance);
    }
}