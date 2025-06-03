
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
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Configuring infrastructure layer's services.
            builder.Services.ConfigureInfrastructure(configuration);
            
            // NOTE: THIS CUSTOM LOGGING MODULE IS BROKEN
            // Configuring custom system loggings
            //builder.Logging.AddSystemLoggingProvider();

            // Configuring application layer's services
            builder.Services.ConfigureApplication(configuration);
            builder.Services.AddControllers();

            // Configuring authentication and authorization with Jwt.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("default_scheme", option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"])),
                    ValidAudiences = configuration.GetSection("Jwt:ValidAudiences").Get<List<string>>(),
                    ValidIssuers = configuration.GetSection("Jwt:ValidIssuers").Get<string[]>()
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
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("allowAllRequest");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
