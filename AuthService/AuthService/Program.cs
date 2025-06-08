
using API.Extentions;
namespace AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices(config);
            builder.Services.AddDatabase(config);
            builder.Services.AddGoogleConfig();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           

            app.UseCookiePolicy();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var check = context.Session.GetString("session_test");
                if (string.IsNullOrEmpty(check))
                {
                    context.Session.SetString("session_test", "hello");
                    Console.WriteLine("✅ Session initialized.");
                }
                else
                {
                    Console.WriteLine("✅ Session found: " + check);
                }
                await next();
            });
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.MapControllers();
            app.Run();
        }
    }
}
