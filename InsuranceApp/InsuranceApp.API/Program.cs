using InsuranceApp.API.Extensions.Pipeline;
using InsuranceApp.API.Extensions.Services;
using InsuranceApp.Application;
using InsuranceApp.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddApiControllers(); // controllere, JWT, Swagger, CORS, pipeline-ul HTTP
builder.Services.AddApplication(); // înregistrează servicii de business, validatoare FluentValidation, AutoMapper
builder.Services.AddInfrastructure(builder.Configuration); // EF Core + SQL Server, repository-uri, autentificare, job-ul de expirare a polițelor

builder.Services.AddIdentityInfrastructure();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJwt();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseApiPipeline(); // se construiește aplicația propriu-zisă și se definește traseul fiecărui request
/* 
 * Request
 * - HTTPS redirect -> forțează clientul să folosească o conexiune securizată
 *                  -> criptează traficul dintre frontend și API
 * - CORS -> Cross-Origin Resource Sharing = browserul consideră diferite două aplicații dacă au alt protocol, domeniu sau port
 *        -> CORS este o protecție aplicată pe browser, nu este autentificare și nu înlocuiește JWT, decide doar ce pagini web au voie să citească răspunsurile API-ului din browser
 * - tratarea centralizată a excepțiilor
 * - autentificare JWT
 * - autorizare
 * - controller
 */

await app.RunAsync();

// Program.cs - conectează componentele (bun exemplu de composition root)