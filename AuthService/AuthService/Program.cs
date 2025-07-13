using DotNetEnv;
using API.Extentions;

namespace AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");

            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Configuration.AddEnvironmentVariables();

            //builder.Configuration["App:FrontendBaseUrl"] = Environment.GetEnvironmentVariable("APP_FRONTEND_BASE_URL");
            //builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_KEY");
            //builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER");
            //builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            //builder.Configuration["Email:SmtpServer"] = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER");
            //builder.Configuration["Email:SmtpPort"] = Environment.GetEnvironmentVariable("EMAIL_SMTP_PORT");
            //builder.Configuration["Email:SenderEmail"] = Environment.GetEnvironmentVariable("EMAIL_SENDER_EMAIL");
            //builder.Configuration["Email:SenderName"] = Environment.GetEnvironmentVariable("EMAIL_SENDER_NAME");
            //builder.Configuration["Email:SenderPassword"] = Environment.GetEnvironmentVariable("EMAIL_SENDER_PASSWORD");

            //builder.Configuration["Authentication:Google:ClientId"] = Environment.GetEnvironmentVariable("AUTH_GOOGLE_CLIENT_ID");
            //builder.Configuration["Authentication:Google:ClientSecret"] = Environment.GetEnvironmentVariable("AUTH_GOOGLE_CLIENT_SECRET");

            //builder.Configuration["Firebase:BucketName"] = Environment.GetEnvironmentVariable("FIREBASE_BUCKET_NAME");
            //builder.Configuration["Firebase:CredentialPath"] = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIAL_PATH");

            //builder.Configuration["ConnectionStrings:AuthConnection"] = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS_AUTHCONNECTION");


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddApplicationServices(config); 
            builder.Services.AddDatabase(config);            
            builder.Services.AddGoogleConfig();              

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 🚦 Middleware pipeline
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowAll");

            app.UseCookiePolicy();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
