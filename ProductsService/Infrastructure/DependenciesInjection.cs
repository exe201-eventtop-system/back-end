using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.Handlers;
using Application.Commons.UoW;
using Infrastructure.Context;
using Infrastructure.Dispatchers;
using Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.FireBase;
using SharedLibrary.Jwt;
using SharedLibrary.Password;
using System.Reflection;

namespace Infrastructure
{
    public static class DependenciesInjection
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Configuring DbContext
            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("SERVICECONNECTION"));
            });


            // Add Firebase Storage
            services.AddScoped<FirebaseStorageService>();


            // Add HttpClients
            services.AddHttpClient("AuthService", client =>
            {
                client.BaseAddress = new Uri(config["AUTHSERVICE:PORT"]);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Note: Using repositories through UnitOfWork so we don't need to register them as services

            /*
            // Configuring repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            */

            // Register unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register dispatchers
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            // Register handlers

            // Scan through the assembly to find all command / query handlers.
            try
            {
                var openHandlerInterfaces = new[]
                {
                    typeof(ICommandHandler<,>),
                    typeof(IQueryHandler<,>)
                };

                var types = Assembly.GetAssembly(typeof(ICommandHandler<,>)).GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface);

                foreach (var impl in types)
                {

                    var matchingInterfaces = impl.GetInterfaces()
                        .Where(i => i.IsGenericType
                                    && openHandlerInterfaces.Contains(i.GetGenericTypeDefinition()));

                    foreach (var serviceType in matchingInterfaces)
                    {
                        services.AddScoped(serviceType, impl);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to register handlers");
            }

            return services;
        }
    }
}
