using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Domain.Entities.Geography;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Geography;

public class CountyRepository(InsuranceDbContext context) : GenericRepository<County>(context), ICountyRepository
{
    public async Task<PageResponse<County>> GetByCountryIdAsync(
        int countryId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default)
    {
        var page = pageRequest.Page;
        var pageSize = pageRequest.PageSize;

        var items = await _context.Counties
            .AsNoTracking()
            .Where(c => c.CountryId == countryId)
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalItems = items.Count;

        return new PageResponse<County>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    public async Task<bool> ExistsAsync(
        int countyId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Counties
            .AsNoTracking()
            .AnyAsync(c => c.Id == countyId, cancellationToken);
    }
}