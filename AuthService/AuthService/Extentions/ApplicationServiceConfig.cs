using Application.Commons.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.SqlServer.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTOs.User;
using SharedLibrary.Email;
using SharedLibrary.FireBase;
using SharedLibrary.Jwt;
using SharedLibrary.Password;
using SharedLibrary.System.APICall;
using System.Security.Claims;
using System.Text;

namespace API.Extentions
{
    public static class ApplicationServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSettings>(config.GetSection("EMAIL"));
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
            services.AddScoped<FirebaseStorageService>();
            services.AddScoped<IPasswordHasher<UserToHashPassword>, PasswordHasher<UserToHashPassword>>();
            services.AddScoped<PasswordHasherService>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddHttpClient<ApiCaller>();


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
                c.SwaggerDoc("v1", new() { Title = "AuthService API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "AuthCookie";
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

                // Mapping Claims
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture");

                // 👇 THÊM ĐOẠN NÀY để log `state` và lỗi
                googleOptions.Events = new OAuthEvents
                {
                    OnRedirectToAuthorizationEndpoint = context =>
                    {
                        var state = context.Properties.Items.ContainsKey(".xsrf") ? context.Properties.Items[".xsrf"] : "NO STATE";
                        Console.WriteLine($"[OAuth] State (before redirect): {state}");
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        var state = context.Request.Query["state"];
                        var error = context.Failure?.Message;
                        Console.WriteLine($"[OAuth] State (callback): {state}");
                        Console.WriteLine($"[OAuth] Remote failure: {error}");

                        context.Response.Redirect($"/error?message={Uri.EscapeDataString(error ?? "unknown")}");
                        context.HandleResponse(); // tránh redirect mặc định
                        return Task.CompletedTask;
                    }
                };
            });


            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
