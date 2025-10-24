using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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


            // Configuring infrastructure layer's services.
            builder.Services.ConfigureInfrastructure(configuration);
            builder.Services.AddControllers();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    ValidateActor = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"])),
                    //ValidAudiences = configuration.GetSection("Jwt:ValidAudiences").Get<List<string>>(),
                    //ValidIssuers = configuration.GetSection("Jwt:ValidIssuers").Get<string[]>()
                };
            });

            // Configuring CORS policies.
            builder.Services.AddCors(x => x.AddPolicy("allowAllRequest", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod();
            }));

            // Configuring Swashbuckle Swagger.
            builder.Services.AddSwaggerGen(options =>
            {
                // Configuring authentication with JWT support for Swagger UI
                options.AddSecurityDefinition("JwtBearerScheme", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Jwt bearer token (Without the 'Bearer')",
                    Name = "JWT Authentication",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JwtBearerScheme"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("allowAllRequest");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
