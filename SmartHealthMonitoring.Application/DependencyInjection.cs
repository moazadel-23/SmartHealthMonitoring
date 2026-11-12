using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmartHealthMonitoring.Application.Common.Behaviors;

namespace SmartHealthMonitoring.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(CurrentPatientValidationBehavior<,>));
        });

        return services;
    }
}
