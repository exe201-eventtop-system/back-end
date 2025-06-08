using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class AddDatabaseConfig
    {
        public static IServiceCollection AddDatabase(
    this IServiceCollection services,
    IConfiguration config)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("AuthConnection")));
            return services;
        }

    }
}
