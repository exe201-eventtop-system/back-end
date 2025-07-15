using Application.Commons.Dispatchers;
using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.Handlers;
using Application.Commons.Interfaces.ApiCaller;
using Application.Commons.UoW;
using Infrastructure.Context;
using Infrastructure.Services;
using Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.FireBase;
using SharedLibrary.Jwt;
using SharedLibrary.PaymentServices;
using SharedLibrary.System.APICall;
using System.Reflection;

namespace Infrastructure
{
    public static class DependenciesInjection
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ScheduledEventServiceDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("EVENTCONNECTION")));

            // Add HttpClients
            services.AddHttpClient("AuthService", client =>
            {
                client.BaseAddress = new Uri(configuration["AUTHSERVICE:PORT"]);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient("ProductService", client =>
            {
                client.BaseAddress = new Uri(configuration["PRODUCTSERVICE:PORT"]);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient("BlogService", client =>
            {
                client.BaseAddress = new Uri(configuration["BLOGSERVICE:PORT"]);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Add Shared library
            services.AddScoped<ApiCaller>();
            services.AddScoped<JwtService>();
            services.AddScoped<PayOSService>();

            // Adding HttpClient used in calling other endpoints
            services.AddScoped<IApiEndpointCaller, ApiEndpointCaller>();

            // Add Firebase Storage
            services.AddScoped<FirebaseStorageService>();

            // Configuring unit of work to centralize calling to the databae.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add command and query dispatchers
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            // Add command and query handlers
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
                    Console.WriteLine(impl.FullName);

                    var matchingInterfaces = impl.GetInterfaces()
                        .Where(i => i.IsGenericType
                                    && openHandlerInterfaces.Contains(i.GetGenericTypeDefinition()));

                    foreach (var serviceType in matchingInterfaces)
                    {
                        Console.WriteLine($"\t{serviceType.FullName}");
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
