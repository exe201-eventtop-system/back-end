using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Net.payOS;
using Repositories;
using Services;
using Services.DTOs;
using Services.Mapping;
using SharedLibrary.DTOs.User;
using SharedLibrary.Jwt;
using SharedLibrary.PaymentServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ServiceClient>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IPasswordHasher<UserToHashPassword>, PasswordHasher<UserToHashPassword>>();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddHttpClient<ServiceClient>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<PayOSService>();




builder.Services.AddHttpClient<ServiceClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
