using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Application.Services.Metadata;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class CurrencyServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<ICurrencyRepository> CurrencyRepository { get; } = new(MockBehavior.Strict);
    public Mock<IPolicyRepository> PolicyRepository { get; } = new(MockBehavior.Strict);
    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public CurrencyService Sut { get; }

    public CurrencyServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.Currencies).Returns(CurrencyRepository.Object);
        UnitOfWork.SetupGet(u => u.Policies).Returns(PolicyRepository.Object);

        Sut = new CurrencyService(UnitOfWork.Object, Mapper.Object, NullLogger<CurrencyService>.Instance);
    }
}