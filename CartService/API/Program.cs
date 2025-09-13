using API.Extentions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Net.payOS;
using Repositories;
using Services;
using Services.DTOs;
using SharedLibrary.DTOs.User;
using SharedLibrary.Jwt;
using SharedLibrary.PaymentServices;
using static Org.BouncyCastle.Math.EC.ECCurve;

DotNetEnv.Env.Load("../../.env");
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ServiceClient>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddDatabase(config);
builder.Services.AddScoped<IPasswordHasher<UserToHashPassword>, PasswordHasher<UserToHashPassword>>();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddHttpClient<ServiceClient>();


builder.Services.AddHttpClient<ServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8080"); 
});



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
