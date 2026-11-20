using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.Client;
using InsuranceApp.Domain.Enums;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class ClientServiceTests
{
    private readonly ClientServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task SearchAsync_WhenRequestIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _fx.Sut.SearchAsync(null!, new PageRequest(), Ct));

        // Assert
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task GetByIdAsync_WhenClientNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _fx.ClientRepository
            .Setup(r => r.GetByIdAsync(123, Ct))
            .ReturnsAsync((Client?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.GetByIdAsync(123, Ct));

        // Assert
        Assert.Equal("Client with id 123 not found.", ex.Message);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenIdentificationNumberNotUnique_ShouldThrowConflict()
    {
        // Arrange
        var request = ClientTestData.ValidCreateRequest(id: "CNP1", email: "a@b.com", phone: "0711");

        _fx.ClientRepository
            .Setup(r => r.IsIdentificationNumberUniqueAsync("CNP1", null, Ct))
            .ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Client with Identification Number CNP1 already exists.", ex.Message);
        _fx.ClientRepository.Verify(r => r.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenEmailNotUnique_ShouldThrowConflict()
    {
        // Arrange
        var request = ClientTestData.ValidCreateRequest(id: "CNP1", email: "dup@b.com", phone: "0711");

        _fx.ClientRepository.Setup(r => r.IsIdentificationNumberUniqueAsync("CNP1", null, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsEmailUniqueAsync("dup@b.com", null, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Client with Email dup@b.com already exists.", ex.Message);
        _fx.ClientRepository.Verify(r => r.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenPhoneNotUnique_ShouldThrowConflict()
    {
        // Arrange
        var request = ClientTestData.ValidCreateRequest(id: "CNP1", email: "a@b.com", phone: "0999");

        _fx.ClientRepository.Setup(r => r.IsIdentificationNumberUniqueAsync("CNP1", null, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsEmailUniqueAsync("a@b.com", null, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0999", null, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Client with Phone Number 0999 already exists.", ex.Message);
        _fx.ClientRepository.Verify(r => r.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenAllUnique_ShouldNotThrow()
    {
        // Arrange
        var request = ClientTestData.ValidCreateRequest(id: "CNP1", email: "a@b.com", phone: "0711");

        _fx.ClientRepository.Setup(r => r.IsIdentificationNumberUniqueAsync("CNP1", null, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsEmailUniqueAsync("a@b.com", null, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0711", null, Ct)).ReturnsAsync(true);

        _fx.Mapper.Setup(m => m.Map<Client>(request)).Returns(new Client());
        _fx.ClientRepository.Setup(r => r.AddAsync(It.IsAny<Client>(), Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
        _fx.Mapper.Setup(m => m.Map<ClientResponse>(It.IsAny<Client>())).Returns(new ClientResponse());

        // Act
        await _fx.Sut.CreateAsync(request, Ct);

        // Assert
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenClientNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _fx.ClientRepository.Setup(r => r.GetByIdAsync(1, Ct)).ReturnsAsync((Client?)null);

        var request = ClientTestData.ValidUpdateRequest(id: "NEW", email: "a@b.com", phone: "0711");

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync("user1", 1, request, Ct));

        // Assert
        Assert.Equal("Client with id 1 not found.", ex.Message);
        _fx.ClientRepository.Verify(r => r.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdentificationNumberNotUnique_ShouldThrowConflict()
    {
        // Arrange
        _fx.ClientRepository.Setup(r => r.GetByIdAsync(1, Ct))
            .ReturnsAsync(new Client { Id = 1, IdentificationNumber = "OLD" });

        var request = ClientTestData.ValidUpdateRequest(id: "DUP", email: "a@b.com", phone: "0711");

        _fx.ClientRepository.Setup(r => r.IsIdentificationNumberUniqueAsync("DUP", 1, Ct))
            .ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.UpdateAsync("user1", 1, request, Ct));

        // Assert
        Assert.Equal("Client with Identification Number DUP already exists.", ex.Message);
        _fx.ClientRepository.Verify(r => r.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenEmailNotUnique_ShouldThrowConflict()
    {
        // Arrange
        _fx.ClientRepository.Setup(r => r.GetByIdAsync(1, Ct))
            .ReturnsAsync(new Client { Id = 1, IdentificationNumber = "OLD" });

        var request = ClientTestData.ValidUpdateRequest(id: "NEW", email: "dup@b.com", phone: "0711");

        _fx.ClientRepository.Setup(r => r.IsIdentificationNumberUniqueAsync("NEW", 1, Ct))
            .ReturnsAsync(true);

        _fx.ClientRepository.Setup(r => r.IsEmailUniqueAsync("dup@b.com", 1, Ct))
            .ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.UpdateAsync("user1", 1, request, Ct));

        // Assert
        Assert.Equal("Client with Email dup@b.com already exists.", ex.Message);
        _fx.ClientRepository.Verify(r => r.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenPhoneNotUnique_ShouldThrowConflict()
    {
        // Arrange
        _fx.ClientRepository.Setup(r => r.GetByIdAsync(1, Ct))
            .ReturnsAsync(new Client { Id = 1, IdentificationNumber = "OLD" });

        var request = ClientTestData.ValidUpdateRequest(id: "NEW", email: "a@b.com", phone: "0999");

        _fx.ClientRepository.Setup(r => r.IsIdentificationNumberUniqueAsync("NEW", 1, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsEmailUniqueAsync("a@b.com", 1, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0999", 1, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.UpdateAsync("user1", 1, request, Ct));

        // Assert
        Assert.Equal("Client with Phone Number 0999 already exists.", ex.Message);
        _fx.ClientRepository.Verify(r => r.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdentificationNumberChanges_ShouldAddAudit()
    {
        // Arrange
        var existingClient = new Client
        {
            Id = 1,
            IdentificationNumber = "OLD",
            ClientType = ClientType.Individual
        };

        _fx.ClientRepository.Setup(r => r.GetByIdAsync(1, Ct)).ReturnsAsync(existingClient);

        var request = ClientTestData.ValidUpdateRequest(id: "NEW", email: "a@b.com", phone: "0711");

        _fx.ClientRepository.Setup(r => r.IsIdentificationNumberUniqueAsync("NEW", 1, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsEmailUniqueAsync("a@b.com", 1, Ct)).ReturnsAsync(true);
        _fx.ClientRepository.Setup(r => r.IsPhoneNumberUniqueAsync("0711", 1, Ct)).ReturnsAsync(true);

        _fx.Mapper
            .Setup(m => m.Map(request, existingClient))
            .Callback<ClientUpdateRequest, Client>((req, client) =>
            {
                client.IdentificationNumber = req.IdentificationNumber;
            })
            .Returns(existingClient);

        _fx.ClientRepository.Setup(r => r.AddIdentificationNumberAuditAsync(
                It.Is<ClientIdentificationNumberAudit>(a =>
                    a.UserId == "user1"
                    && a.ClientId == 1
                    && a.OldIdentificationNumber == "OLD"
                    && a.NewIdentificationNumber == "NEW"),
                Ct))
            .Returns(Task.CompletedTask);

        _fx.ClientRepository.Setup(r => r.UpdateAsync(existingClient, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(0);
        _fx.Mapper.Setup(m => m.Map<ClientResponse>(existingClient)).Returns(new ClientResponse());

        // Act
        await _fx.Sut.UpdateAsync("user1", 1, request, Ct);

        // Assert
        _fx.ClientRepository.Verify(r => r.AddIdentificationNumberAuditAsync(It.IsAny<ClientIdentificationNumberAudit>(), Ct), Times.Once);
        _fx.ClientRepository.Verify(r => r.UpdateAsync(existingClient, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task GetIdentificationNumberAuditAsync_WhenClientExists_ShouldReturnMappedAudits()
    {
        // Arrange
        _fx.ClientRepository.Setup(r => r.GetByIdAsync(1, Ct)).ReturnsAsync(new Client { Id = 1 });

        var audits = new[]
        {
            new ClientIdentificationNumberAudit
            {
                Id = 10,
                ClientId = 1,
                OldIdentificationNumber = "OLD",
                NewIdentificationNumber = "NEW",
                UserId = "user1",
                ChangedAt = DateTime.UtcNow
            }
        };

        var mapped = new[]
        {
            new ClientIdentificationNumberAuditResponse
            {
                Id = 10,
                ClientId = 1,
                OldIdentificationNumber = "OLD",
                NewIdentificationNumber = "NEW",
                UserId = "user1",
                ChangedAt = audits[0].ChangedAt
            }
        };

        _fx.ClientRepository.Setup(r => r.GetIdentificationNumberAuditAsync(1, Ct)).ReturnsAsync(audits);
        _fx.Mapper.Setup(m => m.Map<IEnumerable<ClientIdentificationNumberAuditResponse>>(audits)).Returns(mapped);

        // Act
        var result = await _fx.Sut.GetIdentificationNumberAuditAsync(1, Ct);

        // Assert
        Assert.Equal(mapped, result);
    }
}
