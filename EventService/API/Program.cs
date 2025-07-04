using API.Middlewares;
using Infrastructure;

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
            builder.Configuration["PayOS:ClientId"] = Environment.GetEnvironmentVariable("PAYOS_CLIENTID");
            builder.Configuration["PayOS:ApiKey"] = Environment.GetEnvironmentVariable("PAYOS_APIKEY");
            builder.Configuration["PayOS:ChecksumKey"] = Environment.GetEnvironmentVariable("PAYOS_CHECKSUMKEY");
            builder.Configuration["PayOS:ReturnUrl"] = Environment.GetEnvironmentVariable("PAYOS_RETURNURL");
            builder.Configuration["ServiceUrls:ApiGateway"] = Environment.GetEnvironmentVariable("SERVICEURLS_APIGATEWAY");

            builder.Services.ConfigureInfrastructure(configuration)
                .ConfigureMiddlewares(configuration)
                .AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
