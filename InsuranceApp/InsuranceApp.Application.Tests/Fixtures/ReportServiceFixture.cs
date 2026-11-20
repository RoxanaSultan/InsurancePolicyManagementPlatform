using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Application.Services.Report;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Fixtures;

public sealed class ReportServiceFixture
{
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    public Mock<IPolicyRepository> PolicyRepository { get; } = new(MockBehavior.Strict);
    public Mock<AutoMapper.IMapper> Mapper { get; } = new(MockBehavior.Strict);

    public ReportService Sut { get; }

    public ReportServiceFixture()
    {
        UnitOfWork.SetupGet(u => u.Policies).Returns(PolicyRepository.Object);
        Sut = new ReportService(UnitOfWork.Object, NullLogger<ReportService>.Instance);
    }
}