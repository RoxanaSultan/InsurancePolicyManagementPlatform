using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Client;
using InsuranceApp.Application.Services.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class ClientServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IClientRepository> ClientRepository { get; } = new(MockBehavior.Strict);
    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public ClientService Sut { get; }

    public ClientServiceFixture()
    {
        UnitOfWork.SetupGet(uow => uow.Clients).Returns(ClientRepository.Object);
        Sut = new ClientService(UnitOfWork.Object, Mapper.Object, NullLogger<ClientService>.Instance);
    }
}