using FluentValidation.AspNetCore;
using InsuranceApp.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace InsuranceApp.API.Extensions.Services;

public static class ControllersExtensions
{
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddNewtonsoftJson(o =>
                o.SerializerSettings.Converters.Add(
                    new Newtonsoft.Json.Converters.StringEnumConverter()));

        services.AddFluentValidationAutoValidation();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors
                            .Select(e => e.ErrorMessage)
                            .ToArray()
                    );

                var response = new ErrorResponse
                {
                    ErrorType = "ValidationError",
                    ErrorMessage = "One or more validation errors occurred.",
                    Errors = errors
                };

                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }
}