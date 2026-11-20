using BrokerEntity = InsuranceApp.Domain.Entities.User.Broker;

namespace InsuranceApp.Application.Interfaces.Repositories.Broker;

public interface IBrokerRepository : IRepository<BrokerEntity>
{
    Task<BrokerEntity?> GetByEmailAsync(
        string email, 
        CancellationToken cancellationToken = default);
    Task<BrokerEntity?> GetByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(
        string email, 
        int? excludeBrokerId = null, 
        CancellationToken cancellationToken = default);
    Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber, 
        int? excludeBrokerId = null, 
        CancellationToken cancellationToken = default);
    Task<bool> IsCodeUniqueAsync(
        string code, 
        int? excludeBrokerId = null, 
        CancellationToken cancellationToken = default);
}