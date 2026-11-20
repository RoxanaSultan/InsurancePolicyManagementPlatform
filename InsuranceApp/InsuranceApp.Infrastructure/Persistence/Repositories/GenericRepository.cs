using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity>(InsuranceDbContext context) 
    : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly InsuranceDbContext _context = context;
    protected readonly DbSet<TEntity> _set = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default)
        => await _set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public virtual async Task<PageResponse<TEntity>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default)
    {
        var page = pageRequest.Page;
        var pageSize = pageRequest.PageSize;

        var items = await _set.AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalItems = items.Count;

        return new PageResponse<TEntity>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalItems / pageSize
        };
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _set.AddAsync(entity, cancellationToken);

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        => _context.Entry(entity).State = EntityState.Modified;
}