using Microsoft.AspNetCore.Authentication.Cookies;

namespace API.Extentions
{
    public static class GoogleConfig
    {
        public static IServiceCollection AddGoogleConfig(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            return services;
        }
    }
}
