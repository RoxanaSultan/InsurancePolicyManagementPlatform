using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Application.DTOs.Geographies;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.Report;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Policy;

public class PolicyRepository(InsuranceDbContext context)
    : GenericRepository<PolicyEntity>(context), IPolicyRepository
{
    public override async Task<PolicyEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Policies
            .AsNoTracking()
            .Include(c => c.Broker)
            .Include(c => c.Currency)
            .Include(c => c.Building)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<PageResponse<PolicyEntity>> GetPoliciesAsync(
        PolicyFilterRequest filter,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        IQueryable<PolicyEntity> query = _context.Policies.AsNoTracking();

        if (filter.BrokerId.HasValue)
            query = query.Where(p => p.BrokerId == filter.BrokerId.Value);

        if (filter.Status.HasValue)
            query = query.Where(p => p.Status == filter.Status.Value);

        if (filter.StartDate.HasValue)
            query = query.Where(p => p.EndDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(p => p.StartDate <= filter.EndDate.Value);

        query = query.OrderByDescending(p => p.LastUpdateDate);

        return await ToPageAsync(query, pageRequest, cancellationToken);
    }

    public async Task<bool> ExistsActivePolicyForCurrencyAsync(
        int currencyId,
        DateTime nowUtc,
        CancellationToken cancellationToken = default)
    {
        return await _context.Policies.AsNoTracking().AnyAsync(p =>
            p.CurrencyId == currencyId &&
            p.Status == PolicyStatus.Active &&
            p.StartDate <= nowUtc &&
            p.EndDate >= nowUtc, cancellationToken);
    }

    private static async Task<PageResponse<T>> ToPageAsync<T>(
        IQueryable<T> query,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        var page = pageRequest.Page;
        var pageSize = pageRequest.PageSize;
         
        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PageResponse<T>
        {
            Items = items, 
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalItems / pageSize
        };
    }

    private static IQueryable<PolicyEntity> ApplyReportFilter(
        IQueryable<PolicyEntity> query,
        ReportFilterRequest request)
    {
        query = query.Where(p => p.EndDate >= request.From && p.StartDate <= request.To);

        if (request.Status.HasValue)
        {
            var status = (PolicyStatus)request.Status.Value;
            query = query.Where(p => p.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(request.Currency))
            query = query.Where(p => p.Currency.Code == request.Currency);

        if (request.BuildingType.HasValue)
            query = query.Where(p => p.Building.BuildingType == request.BuildingType.Value); 

        return query;
    }

    public Task<PageResponse<ReportByCountryResponse>> GetPoliciesByCountryAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = ApplyReportFilter(_context.Policies.AsNoTracking(), request);

        var groupedQuery = baseQuery
            .GroupBy(p => new
            {
                p.Building.City.County.CountryId,
                CountryName = p.Building.City.County.Country.Name,
                CurrencyId = p.Currency.Id,
                CurrencyCode = p.Currency.Code,
                CurrencyRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new ReportByCountryResponse
            {
                Country = new CountryResponse
                {
                    Id = g.Key.CountryId,
                    Name = g.Key.CountryName
                },
                Currency = new CurrencyResponse
                {
                    Id = g.Key.CurrencyId,
                    Code = g.Key.CurrencyCode
                },
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(p => p.CalculatedPremiumAmount),
                TotalFinalPremiumInBaseCurrency = g.Sum(p => p.CalculatedPremiumAmount * g.Key.CurrencyRate)
            })
            .OrderBy(r => r.Country.Name)
            .ThenBy(r => r.Currency.Code);

        return ToPageAsync(groupedQuery, pageRequest, cancellationToken);
    }

    public Task<PageResponse<ReportByCountyResponse>> GetPoliciesByCountyAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = ApplyReportFilter(_context.Policies.AsNoTracking(), request);

        var groupedQuery = baseQuery
            .GroupBy(p => new
            {
                p.Building.City.CountyId,
                CountyName = p.Building.City.County.Name,
                CountyCode = p.Building.City.County.Code,
                CurrencyId = p.Currency.Id,
                CurrencyCode = p.Currency.Code,
                CurrencyRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new ReportByCountyResponse
            {
                County = new CountyResponse
                {
                    Id = g.Key.CountyId,
                    Name = g.Key.CountyName,
                    Code = g.Key.CountyCode
                },
                Currency = new CurrencyResponse
                {
                    Id = g.Key.CurrencyId,
                    Code = g.Key.CurrencyCode
                },
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(p => p.CalculatedPremiumAmount),
                TotalFinalPremiumInBaseCurrency = g.Sum(p => p.CalculatedPremiumAmount * g.Key.CurrencyRate)
            })
            .OrderBy(r => r.County.Name)
            .ThenBy(r => r.Currency.Code);

        return ToPageAsync(groupedQuery, pageRequest, cancellationToken);
    }

    public Task<PageResponse<ReportByCityResponse>> GetPoliciesByCityAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = ApplyReportFilter(_context.Policies.AsNoTracking(), request);

        var groupedQuery = baseQuery
            .GroupBy(p => new
            {
                p.Building.CityId,
                CityName = p.Building.City.Name,
                CurrencyId = p.Currency.Id,
                CurrencyCode = p.Currency.Code,
                CurrencyRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new ReportByCityResponse
            {
                City = new CityResponse
                {
                    Id = g.Key.CityId,
                    Name = g.Key.CityName
                },
                Currency = new CurrencyResponse
                {
                    Id = g.Key.CurrencyId,
                    Code = g.Key.CurrencyCode
                },
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(p => p.CalculatedPremiumAmount),
                TotalFinalPremiumInBaseCurrency = g.Sum(p => p.CalculatedPremiumAmount * g.Key.CurrencyRate)
            })
            .OrderBy(r => r.City.Name)
            .ThenBy(r => r.Currency.Code);

        return ToPageAsync(groupedQuery, pageRequest, cancellationToken);
    }

    public Task<PageResponse<ReportByBrokerResponse>> GetPoliciesByBrokerAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = ApplyReportFilter(_context.Policies.AsNoTracking(), request);

        var groupedQuery = baseQuery
            .GroupBy(p => new
            {
                p.BrokerId,
                BrokerCode = p.Broker.Code,
                BrokerName = p.Broker.Name,
                CurrencyId = p.Currency.Id,
                CurrencyCode = p.Currency.Code,
                CurrencyRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new ReportByBrokerResponse
            {
                Broker = new BrokerResponse
                {
                    Id = g.Key.BrokerId,
                    Code = g.Key.BrokerCode,
                    Name = g.Key.BrokerName
                },
                Currency = new CurrencyResponse
                {
                    Id = g.Key.CurrencyId,
                    Code = g.Key.CurrencyCode
                },
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(p => p.CalculatedPremiumAmount),
                TotalFinalPremiumInBaseCurrency = g.Sum(p => p.CalculatedPremiumAmount * g.Key.CurrencyRate)
            })
            .OrderBy(r => r.Broker.Name)
            .ThenBy(r => r.Currency.Code);

        return ToPageAsync(groupedQuery, pageRequest, cancellationToken);
    }

    public async Task<IReadOnlyList<PolicyEntity>> GetPoliciesToExpireAsync(DateTime utcNow, CancellationToken cancellationToken = default)
    {
        return await _context.Policies
            .Where(p => p.Status == PolicyStatus.Active && p.EndDate <= utcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<PolicyEntity> policies, CancellationToken cancellationToken = default)
    {
        _context.Policies.UpdateRange(policies);
    }
}