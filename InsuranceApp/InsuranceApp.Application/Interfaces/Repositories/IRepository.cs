using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Common;

namespace InsuranceApp.Application.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
    Task<PageResponse<TEntity>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);

    Task AddAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default);
    Task UpdateAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default);
}