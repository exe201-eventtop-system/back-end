using Infrastructure.SqlServer.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class AddDatabaseConfig
    {
        public static IServiceCollection AddDatabase(
    this IServiceCollection services,
    IConfiguration config)
        {
            services.AddDbContext<PlanningServiceDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("PLANNINGCONNECTION")));
            return services;
        }

    }
}
