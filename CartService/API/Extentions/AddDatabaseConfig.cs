using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;

namespace API.Extentions
{
    public static class AddDatabaseConfig
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration config)
        {
            var connectionString = config.GetConnectionString("CARTCONNECTION");
            services.AddDbContext<CartServiceDBContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
