using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Domain.Entities.Geography;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Geography;

public class CityRepository(InsuranceDbContext context) : GenericRepository<City>(context), ICityRepository
{
    public async Task<PageResponse<City>> GetByCountyIdAsync(
        int countyId, PageRequest pageRequest, 
        CancellationToken cancellationToken = default)
    {
        var page = pageRequest.Page;
        var pageSize = pageRequest.PageSize;

        var items = await _context.Cities
            .AsNoTracking()
            .Where(c => c.CountyId == countyId)
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalItems = items.Count;

        return new PageResponse<City>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    public async Task<bool> ExistsAsync(
        int cityId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Cities
            .AsNoTracking()
            .AnyAsync(c => c.Id == cityId, cancellationToken);
    }
}