using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations;
using Infrastructure.SystemLogger;
using Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static ILoggingBuilder AddSystemLoggingProvider(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, SystemLoggerProvider>();
            return builder;
        }

        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Configuring DbContext
            services.AddDbContext<ProductServiceDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultDatabase"));
            });

            // Note: Using repositories through UnitOfWork so we don't need to register them as services
            
            /*
            // Configuring repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            */

            // Configuring UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
