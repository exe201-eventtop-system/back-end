using API.Extentions;
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
using static Org.BouncyCastle.Math.EC.ECCurve;


var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load("../../.env");
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.
builder.Configuration["PayOS:ClientId"] = Environment.GetEnvironmentVariable("PAYOS_CLIENTID");
builder.Configuration["PayOS:ApiKey"] = Environment.GetEnvironmentVariable("PAYOS_APIKEY");
builder.Configuration["PayOS:ChecksumKey"] = Environment.GetEnvironmentVariable("PAYOS_CHECKSUMKEY");
builder.Configuration["PayOS:ReturnUrl"] = Environment.GetEnvironmentVariable("PAYOS_RETURNURL");
builder.Configuration["ServiceUrls:ApiGateway"] = Environment.GetEnvironmentVariable("SERVICEURLS_APIGATEWAY");
builder.Configuration["ConnectionStrings:CartConnection"] = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS_CARTCONNECTION");

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
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<PayOSService>();
builder.Services.Configure<PayOSSettings>(builder.Configuration.GetSection("PayOS"));



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
