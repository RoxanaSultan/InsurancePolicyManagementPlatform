using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Broker;
using InsuranceApp.Application.Interfaces.Repositories.Building;
using InsuranceApp.Application.Interfaces.Repositories.Client;
using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Application.Interfaces.Services.Authentication;
using InsuranceApp.Infrastructure.BackgroundJobs;
using InsuranceApp.Infrastructure.Persistence;
using InsuranceApp.Infrastructure.Persistence.Repositories;
using InsuranceApp.Infrastructure.Persistence.Repositories.Broker;
using InsuranceApp.Infrastructure.Persistence.Repositories.Building;
using InsuranceApp.Infrastructure.Persistence.Repositories.Client;
using InsuranceApp.Infrastructure.Persistence.Repositories.Geography;
using InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;
using InsuranceApp.Infrastructure.Persistence.Repositories.Policy;
using InsuranceApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InsuranceDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IBuildingRepository, BuildingRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICountyRepository, CountyRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IFeeConfigurationRepository, FeeConfigurationRepository>();
        services.AddScoped<IRiskFactorConfigurationRepository, RiskFactorConfigurationRepository>();
        services.AddScoped<IBrokerRepository, BrokerRepository>();
        services.AddScoped<IPolicyRepository, PolicyRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHostedService<PolicyExpiryHostedService>();

        return services;
    }
}
