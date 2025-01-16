using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Stopify.Repository;
using Stopify.Service;

namespace Stopify
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, Assembly assembly)
        {
            // Register DB Context
            services.AddScoped<ApplicationDbContext>();

            // Register Repositories
            var repositoryTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)));

            foreach (var repositoryType in repositoryTypes)
            {
                services.AddScoped(repositoryType);
            }

            // Register Services
            var serviceTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IService).IsAssignableFrom(type));

            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType);
            }
        }
    }
}