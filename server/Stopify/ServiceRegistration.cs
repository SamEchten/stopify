using System.Reflection;
using Stopify.Repository;
using Stopify.Service;

namespace Stopify;

public static class ServiceRegistration
{
    public static void RegisterServices(WebApplicationBuilder builder, Assembly assembly)
    {
        var services = builder.Services;

        // Register DB Context
        services.AddScoped<ApplicationDbContext>();

        // Register App Settings
        services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        // Auto Register Repositories
        var repositoryTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)));

        foreach (var repositoryType in repositoryTypes)
        {
            services.AddScoped(repositoryType);
        }

        // Auto Register Services
        var serviceTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IService).IsAssignableFrom(type));

        foreach (var serviceType in serviceTypes)
        {
            services.AddScoped(serviceType);
        }

        // Auto Register Factories
        var factoryTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IFactory).IsAssignableFrom(type));

        foreach (var factoryType in factoryTypes)
        {
            services.AddScoped(factoryType);
        }
    }
}
