using InsuranceApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace InsuranceApp.API.Extensions.Services;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services
            .AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<InsuranceDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}