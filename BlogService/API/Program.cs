using API.Extensions;
using Application;
using Infrastructure;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddEnvironmentVariables();
            var configuration = builder.Configuration;

            builder.Services.ConfigureInfratructure(configuration)
                .ConfigureApplicationServices(configuration)
                .ConfigureMiddlewares(configuration);
            builder.Services.AddControllers();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("allow_all");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
