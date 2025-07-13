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
            string item = config.GetConnectionString("PLANNINGCONNECTION");

            if (string.IsNullOrEmpty(item))
            {
                throw new InvalidProgramException("Lmao");
            }
            Console.WriteLine(item);

            services.AddDbContext<PlanningServiceDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("PLANNINGCONNECTION")));
            return services;
        }

    }
}
