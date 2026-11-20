using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Building;
using InsuranceApp.Application.Interfaces.Repositories.Client;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Application.Services.Building;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class BuildingServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IClientRepository> ClientRepository { get; } = new(MockBehavior.Strict);
    public Mock<ICityRepository> CityRepository { get; } = new(MockBehavior.Strict);
    public Mock<IBuildingRepository> BuildingRepository { get; } = new(MockBehavior.Strict);
    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public BuildingService Sut { get; }

    public BuildingServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.Clients).Returns(ClientRepository.Object);
        UnitOfWork.SetupGet(u => u.Cities).Returns(CityRepository.Object);
        UnitOfWork.SetupGet(u => u.Buildings).Returns(BuildingRepository.Object);

        Sut = new BuildingService(UnitOfWork.Object, Mapper.Object, NullLogger<BuildingService>.Instance);
    }
}