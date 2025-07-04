using Application.Interfaces;
using Application.Usecase;
using Domain.Interfaces;
using Infrastructure.SqlServer.Data;
using Infrastructure.SqlServer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using SharedLibrary.AIGenerate;
using SharedLibrary.DTOs.User;
using SharedLibrary.Jwt;

namespace API.Extentions
{
    public static class ApplicationServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<GeminiSettings>(config.GetSection("Gemini"));

            services.AddHttpClient<GeminiClient>();

            services.AddScoped<IPlanningUseCase, PlanningUseCase>();
            services.AddScoped<IPlanningRepository, PlanningRepository>();
            services.AddScoped<IAIGennerateUseCase, AIGennerateUseCase>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<JwtService>();
            services.AddScoped(typeof(IPasswordHasher<UserToHashPassword>), typeof(PasswordHasher<UserToHashPassword>));

            return services;
        }
    }
}
