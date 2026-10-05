using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Measurements.Commands.CreateReading;
using SmartHealthMonitoring.Infrastructure;

namespace SmartHealthMonitoring.API.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationAndApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Add Infrastructure Services (DbContext & Repositories)
        services.AddInfrastructureServices(configuration);

        // 2. Register MediatR for Application layer
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateReadingCommands).Assembly));

        // 3. Register OpenAPI / Scalar
        services.AddOpenApi();

        // 4. Register Controllers & Custom Model Validation Response Format
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
