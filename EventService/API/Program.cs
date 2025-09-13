using API.Middlewares;
using Infrastructure;
using SharedLibrary.Jwt;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.ConfigureInfrastructure(configuration)
                .ConfigureMiddlewares(configuration)
                .AddControllers();
            builder.Services.AddScoped<JwtService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
