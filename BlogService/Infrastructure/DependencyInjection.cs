using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfratructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BlogServiceDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("BlogConnection")));
            services.AddScoped<IBlogRepository, BlogRepository>();

            return services;
        }
    }
}
