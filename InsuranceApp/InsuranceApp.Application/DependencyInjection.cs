using FluentValidation;
using InsuranceApp.Application.Interfaces.Services.Broker;
using InsuranceApp.Application.Interfaces.Services.Building;
using InsuranceApp.Application.Interfaces.Services.Client;
using InsuranceApp.Application.Interfaces.Services.Geography;
using InsuranceApp.Application.Interfaces.Services.Metadata;
using InsuranceApp.Application.Interfaces.Services.Policy;
using InsuranceApp.Application.Interfaces.Services.Report;
using InsuranceApp.Application.Services.Broker;
using InsuranceApp.Application.Services.Building;
using InsuranceApp.Application.Services.Client;
using InsuranceApp.Application.Services.Geography;
using InsuranceApp.Application.Services.Metadata;
using InsuranceApp.Application.Services.Policy;
using InsuranceApp.Application.Services.Report;
using InsuranceApp.Application.Validators.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>(); // când un controller cere un IClientService, construiește un ClientService și păstrează aceeași instanță pe durata requestului HTTP
        services.AddScoped<IBuildingService, BuildingService>();
        services.AddScoped<IGeographyService, GeographyService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IFeeConfigurationService, FeeConfigurationService>();
        services.AddScoped<IRiskFactorConfigurationService, RiskFactorConfigurationService>();
        services.AddScoped<IBrokerService, BrokerService>();
        services.AddScoped<IPolicyService, PolicyService>();
        services.AddScoped<IPolicyCancellationService, PolicyCancellationService>();
        services.AddScoped<IReportService, ReportService>();

        services.AddValidatorsFromAssemblyContaining<ClientCreateRequestValidator>();
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return services;
    }
}
