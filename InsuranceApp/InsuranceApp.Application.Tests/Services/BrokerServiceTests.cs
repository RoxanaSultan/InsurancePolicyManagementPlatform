using AutoMapper;
using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Domain.Enums;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class BrokerServiceTests
{
    private readonly BrokerServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task ActivateAsync_WhenBrokerNotFound_ShouldThrowNotFound()
    {
        // Arrange
        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(10, Ct)).ReturnsAsync((Broker?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.ActivateAsync(10, Ct));

        // Assert
        Assert.Equal("Broker with ID 10 not found.", ex.Message);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ActivateAsync_WhenBrokerExists_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var broker = BrokerTestData.BrokerEntity(id: 10, status: BrokerStatus.Inactive);

        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(10, Ct)).ReturnsAsync(broker);
        _fx.BrokerRepository.Setup(r => r.UpdateAsync(broker, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new BrokerResponse();
        _fx.Mapper.Setup(m => m.Map<BrokerResponse>(broker)).Returns(response);

        // Act
        var result = await _fx.Sut.ActivateAsync(10, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.Equal(BrokerStatus.Active, broker.Status);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(broker, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task DeactivateAsync_WhenBrokerNotFound_ShouldThrowNotFound()
    {
        // Arrange
        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(11, Ct)).ReturnsAsync((Broker?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.DeactivateAsync(11, Ct));

        // Assert
        Assert.Equal("Broker with ID 11 not found.", ex.Message);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeactivateAsync_WhenBrokerExists_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var broker = BrokerTestData.BrokerEntity(id: 11, status: BrokerStatus.Active);

        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(11, Ct)).ReturnsAsync(broker);
        _fx.BrokerRepository.Setup(r => r.UpdateAsync(broker, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new BrokerResponse();
        _fx.Mapper.Setup(m => m.Map<BrokerResponse>(broker)).Returns(response);

        // Act
        var result = await _fx.Sut.DeactivateAsync(11, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.Equal(BrokerStatus.Inactive, broker.Status);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(broker, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenEmailNotUnique_ShouldThrowConflict()
    {
        // Arrange
        var request = BrokerTestData.ValidCreateRequest(email: "dup@broker.com", phone: "0711");

        _fx.BrokerRepository.Setup(r => r.IsEmailUniqueAsync("dup@broker.com", null, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Broker with Email dup@broker.com already exists.", ex.Message);
        _fx.BrokerRepository.Verify(r => r.AddAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenPhoneNotUnique_ShouldThrowConflict()
    {
        // Arrange
        var request = BrokerTestData.ValidCreateRequest(email: "a@broker.com", phone: "0777");

        _fx.BrokerRepository.Setup(r => r.IsEmailUniqueAsync("a@broker.com", null, Ct)).ReturnsAsync(true);
        _fx.BrokerRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0777", null, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Broker with Phone Number 0777 already exists.", ex.Message);
        _fx.BrokerRepository.Verify(r => r.AddAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenUnique_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var request = BrokerTestData.ValidCreateRequest(email: "a@broker.com", phone: "0711");

        _fx.BrokerRepository.Setup(r => r.IsEmailUniqueAsync("a@broker.com", null, Ct)).ReturnsAsync(true);
        _fx.BrokerRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0711", null, Ct)).ReturnsAsync(true);
        _fx.BrokerRepository.Setup(r => r.IsCodeUniqueAsync(It.IsAny<string>(), null, Ct)).ReturnsAsync(true);

        var broker = new Broker();
        _fx.Mapper.Setup(m => m.Map<Broker>(request)).Returns(broker);

        _fx.BrokerRepository.Setup(r => r.AddAsync(broker, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new BrokerResponse();
        _fx.Mapper.Setup(m => m.Map<BrokerResponse>(broker)).Returns(response);

        // Act
        var result = await _fx.Sut.CreateAsync(request, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.BrokerRepository.Verify(r => r.AddAsync(broker, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var page = new PageRequest();

        var brokers = new List<Broker> { new Broker { Id = 5 } };
        var pageResponse = new PageResponse<Broker>
        {
            Items = brokers,
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _fx.BrokerRepository.Setup(r => r.GetAllAsync(page, Ct)).ReturnsAsync(pageResponse);

        IReadOnlyList<BrokerResponse> mapped = new List<BrokerResponse> { new BrokerResponse() };
        _fx.Mapper.Setup(m => m.Map<IReadOnlyList<BrokerResponse>>(brokers)).Returns(mapped);

        // Act
        var result = await _fx.Sut.GetAllAsync(page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        _fx.BrokerRepository.Verify(r => r.GetAllAsync(page, Ct), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBrokerNotFound_ShouldThrowNotFound()
    {
        // Arrange
        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(99, Ct)).ReturnsAsync((Broker?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.GetByIdAsync(99, Ct));

        // Assert
        Assert.Equal("Broker with ID 99 not found.", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBrokerExists_ShouldReturnDetailsResponse()
    {
        // Arrange
        var broker = BrokerTestData.BrokerEntity(id: 99);

        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(99, Ct)).ReturnsAsync(broker);

        var response = new BrokerDetailsResponse();
        _fx.Mapper.Setup(m => m.Map<BrokerDetailsResponse>(broker)).Returns(response);

        // Act
        var result = await _fx.Sut.GetByIdAsync(99, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.BrokerRepository.Verify(r => r.GetByIdAsync(99, Ct), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenBrokerNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = BrokerTestData.ValidUpdateRequest(email: "a@broker.com", phone: "0711");

        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(42, Ct)).ReturnsAsync((Broker?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync(42, request, Ct));

        // Assert
        Assert.Equal("Broker with ID 42 not found.", ex.Message);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenEmailNotUnique_ShouldThrowConflict()
    {
        // Arrange
        var existing = BrokerTestData.BrokerEntity(id: 42);
        var request = BrokerTestData.ValidUpdateRequest(email: "dup@broker.com", phone: "0711");

        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(42, Ct)).ReturnsAsync(existing);
        _fx.BrokerRepository.Setup(r => r.IsEmailUniqueAsync("dup@broker.com", 42, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.UpdateAsync(42, request, Ct));

        // Assert
        Assert.Equal("Broker with Email dup@broker.com already exists.", ex.Message);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenPhoneNotUnique_ShouldThrowConflict()
    {
        // Arrange
        var existing = BrokerTestData.BrokerEntity(id: 42);
        var request = BrokerTestData.ValidUpdateRequest(email: "a@broker.com", phone: "0777");

        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(42, Ct)).ReturnsAsync(existing);
        _fx.BrokerRepository.Setup(r => r.IsEmailUniqueAsync("a@broker.com", 42, Ct)).ReturnsAsync(true);
        _fx.BrokerRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0777", 42, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.UpdateAsync(42, request, Ct));

        // Assert
        Assert.Equal("Broker with Phone Number 0777 already exists.", ex.Message);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(It.IsAny<Broker>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenUnique_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var existing = BrokerTestData.BrokerEntity(id: 42);
        var request = BrokerTestData.ValidUpdateRequest(email: "a@broker.com", phone: "0711");

        _fx.BrokerRepository.Setup(r => r.GetByIdAsync(42, Ct)).ReturnsAsync(existing);
        _fx.BrokerRepository.Setup(r => r.IsEmailUniqueAsync("a@broker.com", 42, Ct)).ReturnsAsync(true);
        _fx.BrokerRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0711", 42, Ct)).ReturnsAsync(true);

        _fx.Mapper.Setup(m => m.Map(request, existing)).Returns(existing);
        _fx.BrokerRepository.Setup(r => r.UpdateAsync(existing, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new BrokerResponse();
        _fx.Mapper.Setup(m => m.Map<BrokerResponse>(existing)).Returns(response);

        // Act
        var result = await _fx.Sut.UpdateAsync(42, request, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.BrokerRepository.Verify(r => r.UpdateAsync(existing, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
        _fx.Mapper.Verify(m => m.Map(request, existing), Times.Once);
    }
}