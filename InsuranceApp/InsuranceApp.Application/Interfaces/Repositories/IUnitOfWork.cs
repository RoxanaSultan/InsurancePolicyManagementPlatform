using InsuranceApp.Application.Interfaces.Repositories.Broker;
using InsuranceApp.Application.Interfaces.Repositories.Building;
using InsuranceApp.Application.Interfaces.Repositories.Client;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Application.Interfaces.Repositories.Policy;

namespace InsuranceApp.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IClientRepository Clients { get; }
    IBuildingRepository Buildings { get; }
    ICountryRepository Countries { get; }
    ICountyRepository Counties { get; }
    ICityRepository Cities { get; }
    ICurrencyRepository Currencies { get; }
    IFeeConfigurationRepository FeeConfigurations { get; }
    IRiskFactorConfigurationRepository RiskFactorConfigurations { get; }
    IBrokerRepository Brokers { get; }
    IPolicyRepository Policies { get; }
    IPolicyCancellationRepository PolicyCancellations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}