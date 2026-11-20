using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Broker;
using InsuranceApp.Application.Interfaces.Repositories.Building;
using InsuranceApp.Application.Interfaces.Repositories.Client;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Infrastructure.Persistence.Repositories.Broker;
using InsuranceApp.Infrastructure.Persistence.Repositories.Building;
using InsuranceApp.Infrastructure.Persistence.Repositories.Client;
using InsuranceApp.Infrastructure.Persistence.Repositories.Geography;
using InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;
using InsuranceApp.Infrastructure.Persistence.Repositories.Policy;

namespace InsuranceApp.Infrastructure.Persistence.Repositories;

public class UnitOfWork(InsuranceDbContext context) : IUnitOfWork
{
    private readonly InsuranceDbContext _context = context;

    private ClientRepository? _clients;
    private BuildingRepository? _buildings;
    private CountryRepository? _countries;
    private CountyRepository? _counties;
    private CityRepository? _cities;
    private CurrencyRepository? _currencies;
    private FeeConfigurationRepository? _feeConfigurations;
    private RiskFactorConfigurationRepository? _riskFactorConfigurations;
    private BrokerRepository? _brokers;
    private PolicyRepository? _policies;
    private PolicyCancellationRepository? _policyCancellations;

    public IClientRepository Clients => _clients ??= new ClientRepository(_context);
    public IBuildingRepository Buildings => _buildings ??= new BuildingRepository(_context);
    public ICountryRepository Countries => _countries ??= new CountryRepository(_context);
    public ICountyRepository Counties => _counties ??= new CountyRepository(_context);
    public ICityRepository Cities => _cities ??= new CityRepository(_context);
    public ICurrencyRepository Currencies => _currencies ??= new CurrencyRepository(_context);
    public IFeeConfigurationRepository FeeConfigurations => _feeConfigurations ??= new FeeConfigurationRepository(_context);
    public IRiskFactorConfigurationRepository RiskFactorConfigurations => _riskFactorConfigurations ??= new RiskFactorConfigurationRepository(_context);
    public IBrokerRepository Brokers => _brokers ??= new BrokerRepository(_context);
    public IPolicyRepository Policies => _policies ??= new PolicyRepository(_context);
    public IPolicyCancellationRepository PolicyCancellations => _policyCancellations ??= new PolicyCancellationRepository(_context);
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}