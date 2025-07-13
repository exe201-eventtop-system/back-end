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
            // 🔍 In ra ConnectionString để kiểm tra
            var connectionString = config.GetConnectionString("CARTCONNECTION");
            Console.WriteLine("==> CartConnection: " + connectionString);

            services.AddDbContext<CartServiceDBContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
