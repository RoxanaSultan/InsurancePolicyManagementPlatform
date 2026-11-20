using InsuranceApp.Application.Interfaces.Repositories.Broker;
using BrokerEntity = InsuranceApp.Domain.Entities.User.Broker;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Broker;

public class BrokerRepository(InsuranceDbContext context) : GenericRepository<BrokerEntity>(context), IBrokerRepository
{
    public override async Task<BrokerEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Brokers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<BrokerEntity?> GetByEmailAsync(
        string email, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Brokers
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Email == email, cancellationToken);
    }

    public async Task<BrokerEntity?> GetByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Brokers
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(
        string email, 
        int? excludeBrokerId = null, 
        CancellationToken cancellationToken = default)
    {
        var trimmed = email.Trim();

        return !await _context.Brokers
            .AsNoTracking()
            .AnyAsync(b =>
                b.Id != excludeBrokerId &&
                b.Email != null &&
                b.Email == trimmed, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber, 
        int? excludeBrokerId = null, 
        CancellationToken cancellationToken = default)
    {
        var trimmed = phoneNumber.Trim();

        return !await _context.Brokers
            .AsNoTracking()
            .AnyAsync(b =>
                b.Id != excludeBrokerId &&
                b.Phone != null &&
                b.Phone == trimmed, cancellationToken);
    }

    public async Task<bool> IsCodeUniqueAsync(
        string code, 
        int? excludeBrokerId = null, 
        CancellationToken cancellationToken = default)
    {
        return !await _context.Brokers
            .AsNoTracking()
            .AnyAsync(b =>
                b.Id != excludeBrokerId &&
                b.Code != null &&
                b.Code == code, cancellationToken);
    }
}