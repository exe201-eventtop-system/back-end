using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API.Middlewares
{
    /// <summary>
    ///     Configure application middleware pipeline.
    /// </summary>
    public static class MiddlewaresConfiguration
    {
        /// <summary>
        ///     Configure application middleware services.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">Configurations from appsettings.json and other soruces</param>
        /// <returns>The configured service collection</returns>
        public static IServiceCollection ConfigureMiddlewares(this IServiceCollection services, IConfiguration configuration)
        {
            // General configurations
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "session_cookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.IsEssential = true;
            })
                .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidateActor = false,
                    ValidIssuers = configuration.GetValue<List<string>>("Jwt:ValidIssuers"),
                    ValidAudiences = configuration.GetValue<List<string>>("Jwt:ValidAudiences"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:SigningKey"))
                    )
                };
            });

            // Authorization
            services.AddAuthorization();

            // CORS policies
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            // Session and Caches
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Swagger endpoints for development
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Auth Service API",
                    Version = "v1"
                });

                var scheme = new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition("Bearer", scheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { scheme, Array.Empty<string>() }
                });

                // SwaggerGen documentations
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ApiDocument.xml"));
            });

            return services;
        }
    }
}
