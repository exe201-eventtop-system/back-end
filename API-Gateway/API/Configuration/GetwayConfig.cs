using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using SharedLibrary.Email;
using System.Text;

namespace API.Configuration
{
    public static class GetwayConfig
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddOcelot(config);
            services.AddSwaggerForOcelot(config);

            var jwtSettings = config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],

                        ValidateAudience = true,
                        ValidAudience = jwtSettings["Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero 
                    };
                });

            return services;
        }

    }
}
