using Application.Common.Mapper;
using Application.Contracts;
using Application.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure auto mapper
            services.AddAutoMapper(typeof(CategoryMapperProfile), typeof(ServiceMapperProfile), typeof(PackageMapperProfile));

            // Configure application layer services.
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddHttpClient("AuthService", client =>
            {
                client.BaseAddress = new Uri(configuration["AuthService:BaseUrl"]);
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            return services;
        }
    }
}
