using InsuranceApp.API.Middlewares;

namespace InsuranceApp.API.Extensions.Pipeline;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("Frontend"); // API-ul acceptă request-uri din frontend-ul Vite care rulează pe localhost:5173, cu orice header și orice metodă HTTP

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}