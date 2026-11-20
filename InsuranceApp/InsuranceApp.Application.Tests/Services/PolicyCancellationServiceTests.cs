using InsuranceApp.Application.DTOs.PolicyCancellation;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.Policy;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class PolicyCancellationServiceTests
{
    private static readonly CancellationToken Ct = CancellationToken.None;
    private readonly PolicyCancellationServiceFixture _fx = new();

    private void ArrangeSaveChanges() =>
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

    [Fact]
    public async Task CreateAsync_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var request = PolicyCancellationTestData.ValidCreateRequest();
        var entity = new PolicyCancellation();

        _fx.Mapper.Setup(m => m.Map<PolicyCancellation>(request)).Returns(entity);
        _fx.PolicyCancellationRepository.Setup(r => r.AddAsync(entity, Ct)).Returns(Task.CompletedTask);
        ArrangeSaveChanges();

        var response = new PolicyCancellationResponse();
        _fx.Mapper.Setup(m => m.Map<PolicyCancellationResponse>(entity)).Returns(response);

        // Act
        var result = await _fx.Sut.CreateAsync(10, request, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.Equal(10, entity.PolicyId);
        _fx.PolicyCancellationRepository.Verify(r => r.AddAsync(entity, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }
}