using AutoMapper;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Application.Services.Policy;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class PolicyCancellationServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IPolicyCancellationRepository> PolicyCancellationRepository { get; } = new(MockBehavior.Strict);
    public Mock<IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public PolicyCancellationService Sut { get; }

    public PolicyCancellationServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.PolicyCancellations).Returns(PolicyCancellationRepository.Object);
        Sut = new PolicyCancellationService(UnitOfWork.Object, Mapper.Object, NullLogger<PolicyCancellationService>.Instance);
    }
}