using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace InsuranceApp.IntegrationTests.Infrastructure;

public static class TestDbSeeder
{
    public static void Seed(InsuranceDbContext db)
    {
        if (!db.Roles.Any(r => r.Name == "Broker"))
        {
            db.Roles.Add(new IdentityRole
            {
                Name = "Broker",
                NormalizedName = "BROKER"
            });
        }

        if (!db.Roles.Any(r => r.Name == "Admin"))
        {
            db.Roles.Add(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }

        if (!db.Countries.Any())
        {
            var country = new Country { Id = 1, Name = "Romania" };
            var county = new County { Id = 1, Name = "Bucuresti", Code = "B", CountryId = 1 };
            var city = new City { Id = 1, Name = "Bucuresti", CountyId = 1 };

            db.Countries.Add(country);
            db.Counties.Add(county);
            db.Cities.Add(city);
        }

        db.SaveChanges();
    }
}
