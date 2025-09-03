using Microsoft.Extensions.DependencyInjection;

namespace JuniorBackDotnetProxy.Configuration
{
   
    /// Extension class to configure CORS (Cross-Origin Resource Sharing) policies.
   
    public static class CorsConfig
    {
        /// <param name="services">IServiceCollection for dependency injection</param>
        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ReactAndGitHub", policy =>
                {
                    // Allow requests from these origins
                    policy.WithOrigins(
                            "http://localhost:5173",        // React / Vite development server
                            "https://<tu-github-pages>.github.io" // Production GitHub Pages site
                        )
                        .AllowAnyHeader()  // Allow any HTTP header
                        .AllowAnyMethod(); // Allow any HTTP method (GET, POST, etc.)
                });
            });
        }
    }
}
