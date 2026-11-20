using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Repositories.Client;
using InsuranceApp.Domain.Entities.Client;
using Microsoft.EntityFrameworkCore;
using ClientEntity = InsuranceApp.Domain.Entities.Client.Client;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Client;

public class ClientRepository(InsuranceDbContext context) : GenericRepository<ClientEntity>(context), IClientRepository
{
    public override async Task<ClientEntity?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .AsNoTracking()
            .Include(c => c.Buildings)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ClientEntity>> SearchAsync(
        ClientSearchRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default)
    {
        var page = pageRequest.Page < 1 ? 1 : pageRequest.Page;
        var pageSize = pageRequest.PageSize < 1 ? 20 : pageRequest.PageSize;

        IQueryable<ClientEntity> query = _context.Clients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var n = request.Name.Trim();
            query = query.Where(c => c.Name != null && EF.Functions.Like(c.Name, $"%{n}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
        {
            var idn = request.IdentificationNumber.Trim();
            query = query.Where(c => c.IdentificationNumber != null && c.IdentificationNumber == idn);
        }

        return await query
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsIdentificationNumberUniqueAsync(
        string identificationNumber, 
        int? excludeClientId = null,
        CancellationToken cancellationToken = default)
    {
        var trimmed = identificationNumber.Trim();

        return !await _context.Clients
            .AsNoTracking()
            .AnyAsync(c => c.IdentificationNumber == trimmed && c.Id != excludeClientId, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(
        string email, 
        int? excludeClientId = null, 
        CancellationToken cancellationToken = default)
    {
        var trimmed = email.Trim();

        return !await _context.Clients
            .AsNoTracking()
            .AnyAsync(c =>
                c.Id != excludeClientId &&
                c.ContactInfo != null &&
                c.ContactInfo.Email != null &&
                c.ContactInfo.Email == trimmed, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber, 
        int? excludeClientId = null, 
        CancellationToken cancellationToken = default)
    {
        var trimmed = phoneNumber.Trim();

        return !await _context.Clients
            .AsNoTracking()
            .AnyAsync(c =>
                c.Id != excludeClientId &&
                c.ContactInfo != null &&
                c.ContactInfo.PhoneNumber != null &&
                c.ContactInfo.PhoneNumber == trimmed, cancellationToken);
    }

    public async Task AddIdentificationNumberAuditAsync(
        ClientIdentificationNumberAudit audit, 
        CancellationToken cancellationToken = default)
    {
        await _context.ClientIdentificationNumberAudits.AddAsync(audit, cancellationToken);
    }

    public async Task<IEnumerable<ClientIdentificationNumberAudit>> GetIdentificationNumberAuditAsync(
        int clientId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ClientIdentificationNumberAudits
            .AsNoTracking()
            .Where(audit => audit.ClientId == clientId)
            .OrderByDescending(audit => audit.ChangedAt)
            .ToListAsync(cancellationToken);
    }
}
