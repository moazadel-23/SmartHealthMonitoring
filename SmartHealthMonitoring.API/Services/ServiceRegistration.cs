using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SmartHealthMonitoring.Application;
using SmartHealthMonitoring.Infrastructure;

namespace SmartHealthMonitoring.API.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationAndApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Add Infrastructure Services (DbContext & Repositories)
        services.AddInfrastructureServices(configuration);

        // 2. Add Application Services (MediatR & CurrentPatient validation pipeline)
        services.AddApplicationServices();

        // 3. Register OpenAPI / Scalar
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Servers = new List<OpenApiServer>
                {
                    new() { Url = "http://localhost:5008", Description = "Local Server" }
                };
                return Task.CompletedTask;
            });
        });

        // 4. Register CORS Policy for Frontend Integration
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                                     ?? new[] { "http://localhost:5173", "http://localhost:4173", "http://localhost:3000", "http://127.0.0.1:3000" };

                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        // 5. Register Controllers & Custom Model Validation Response Format
        services.AddCustomControllers();

        return services;
    }

    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorMessage = context.ModelState
                        .Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault();

                    return new BadRequestObjectResult(new
                    {
                        message = errorMessage ?? "Invalid input data."
                    });
                };
            });

        return services;
    }
}
