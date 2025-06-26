using Application.Interfaces;
using Application.Usecase;
using Domain.Interfaces;
using Infrastructure.SqlServer.Data;
using Infrastructure.SqlServer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using SharedLibrary.DTOs.User;
using SharedLibrary.Jwt;

namespace API.Extentions
{
    public static class ApplicationServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddScoped<IPlanningUseCase, PlanningUseCase>();
            services.AddScoped<IPlanningRepository, PlanningRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<JwtService>();
            services.AddScoped(typeof(IPasswordHasher<UserToHashPassword>), typeof(PasswordHasher<UserToHashPassword>));

            return services;
        }
    }
}
