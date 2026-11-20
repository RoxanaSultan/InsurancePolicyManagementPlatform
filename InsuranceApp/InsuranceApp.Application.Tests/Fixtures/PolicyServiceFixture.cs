using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Broker;
using InsuranceApp.Application.Interfaces.Repositories.Building;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Application.Interfaces.Services.Authentication;
using InsuranceApp.Application.Interfaces.Services.Policy;
using InsuranceApp.Application.Services.Policy;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class PolicyServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IPolicyRepository> PolicyRepository { get; } = new(MockBehavior.Strict);
    public Mock<IPolicyCancellationRepository> PolicyCancellationRepository { get; } = new(MockBehavior.Strict);
    public Mock<IBrokerRepository> BrokerRepository { get; } = new(MockBehavior.Strict);
    public Mock<IBuildingRepository> BuildingRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICityRepository> CityRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICountyRepository> CountyRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICountryRepository> CountryRepository { get; } = new(MockBehavior.Strict);
    public Mock<IFeeConfigurationRepository> FeeConfigurationRepository { get; } = new(MockBehavior.Strict);
    public Mock<IRiskFactorConfigurationRepository> RiskFactorRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICurrencyRepository> CurrencyRepository { get; } = new(MockBehavior.Strict);

    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);
    public Mock<ICurrentUserService> CurrentUserService { get; } = new(MockBehavior.Strict);
    public Mock<IPolicyCancellationService> PolicyCancellationService { get; } = new(MockBehavior.Strict);

    public PolicyService Sut { get; }

    public PolicyServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.Policies).Returns(PolicyRepository.Object);
        UnitOfWork.SetupGet(u => u.PolicyCancellations).Returns(PolicyCancellationRepository.Object);
        UnitOfWork.SetupGet(u => u.Brokers).Returns(BrokerRepository.Object);
        UnitOfWork.SetupGet(u => u.Buildings).Returns(BuildingRepository.Object);
        UnitOfWork.SetupGet(u => u.Cities).Returns(CityRepository.Object);
        UnitOfWork.SetupGet(u => u.Counties).Returns(CountyRepository.Object);
        UnitOfWork.SetupGet(u => u.Countries).Returns(CountryRepository.Object);
        UnitOfWork.SetupGet(u => u.FeeConfigurations).Returns(FeeConfigurationRepository.Object);
        UnitOfWork.SetupGet(u => u.RiskFactorConfigurations).Returns(RiskFactorRepository.Object);
        UnitOfWork.SetupGet(u => u.Currencies).Returns(CurrencyRepository.Object);

        Sut = new PolicyService(
            UnitOfWork.Object,
            Mapper.Object,
            CurrentUserService.Object,
            PolicyCancellationService.Object,
            NullLogger<PolicyService>.Instance);
    }
}