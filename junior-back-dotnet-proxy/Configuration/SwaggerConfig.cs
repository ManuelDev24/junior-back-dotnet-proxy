using Microsoft.OpenApi.Models;

namespace junior_back_dotnet_proxy.Configuration
{
  
    /// Extension class to configure Swagger/OpenAPI for API documentation.
   
    public static class SwaggerConfig
    {
      
    
        /// <param name="services">IServiceCollection for dependency injection</param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            // Registers services for discovering endpoints
            services.AddEndpointsApiExplorer();

            // Registers Swagger generator with API info
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Junior Back API",        // API title
                    Version = "v1",                    // API version
                    Description = "API documentation for Junior Back", // API description
                });
            });
        }

        
        /// Configures Swagger middleware to serve Swagger JSON and UI.
       
        public static void UseSwaggerConfiguration(this WebApplication app)
        {
            // Serve Swagger JSON endpoint
            app.UseSwagger();

            // Serve Swagger UI
            app.UseSwaggerUI(c =>
            {
                // Set Swagger endpoint
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Junior Back API v1");

                // Set Swagger UI at root URL (http://localhost:5000/)
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
