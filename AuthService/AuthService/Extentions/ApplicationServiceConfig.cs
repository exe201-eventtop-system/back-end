using Application.Commons.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.DTOs.User;
using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.Password;
using System.Security.Claims;
using System.Text;

namespace API.Extentions
{
    public static class ApplicationServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Path = "/"; 
            });

            // Dependency Injection
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserUseCase, UserUseCase>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<EmailService>();
            services.AddScoped<JwtService>();
            services.AddScoped<IPasswordHasher<UserToHashPassword>, PasswordHasher<UserToHashPassword>>();
            services.AddScoped<PasswordHasherService>();


            // Session & Cache
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Auth Service API",
                    Version = "v1"
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.IsEssential = true;
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
            {
                var googleAuthNSection = config.GetSection("Authentication:Google");
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                googleOptions.CallbackPath = "/api/auth/google-callback";
                googleOptions.Scope.Add("profile");
                googleOptions.Scope.Add("email");
                googleOptions.SaveTokens = true;

                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture");
            });

            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
