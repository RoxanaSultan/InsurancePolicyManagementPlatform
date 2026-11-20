using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Repositories.Building;
using Microsoft.EntityFrameworkCore;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Building;

public class BuildingRepository(InsuranceDbContext context) : GenericRepository<BuildingEntity>(context), IBuildingRepository
{
    public override async Task<BuildingEntity?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Buildings
            .AsNoTracking()
            .Include(b => b.City)
                .ThenInclude(c => c.County)
                    .ThenInclude(co => co.Country)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<PageResponse<BuildingEntity>> GetByClientIdAsync(
        int clientId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default)
    {
        var page = pageRequest.Page;
        var pageSize = pageRequest.PageSize;

        var items = await _context.Buildings
            .AsNoTracking()
            .Where(b => b.ClientId == clientId)
            .OrderBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalItems = await _context.Buildings
            .AsNoTracking()
            .CountAsync(b => b.ClientId == clientId, cancellationToken);

        return new PageResponse<BuildingEntity>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalItems / pageSize
        };
    }
}