using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace JuniorBackDotnetProxy.Api.Middleware
{
    // Custom middleware to handle exceptions globally
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        // Constructor that receives the next middleware in the pipeline
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Main method executed for each HTTP request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle any exception that occurs
                await HandleExceptionAsync(context, ex);
            }
        }

        // Method to handle the exception and return a JSON response
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set response content type to JSON
            context.Response.ContentType = "application/json";

            // Assign HTTP status code based on exception type
            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,    // 400: Validation error
                KeyNotFoundException => (int)HttpStatusCode.NotFound,  // 404: Resource not found
                HttpRequestException => (int)HttpStatusCode.BadGateway, // 502: External API error
                _ => (int)HttpStatusCode.InternalServerError           // 500: Unhandled server error
            };

            // Create an object with the error message
            var response = new { Error = exception.Message };

            // Serialize the object to JSON
            var json = JsonSerializer.Serialize(response);

            // Return the HTTP response with the JSON
            return context.Response.WriteAsync(json);
        }
    }

    // Extension to easily register the middleware in Program.cs
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
