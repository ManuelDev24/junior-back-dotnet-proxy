using FluentValidation;
using JuniorBackDotnetProxy.Api.Dtos;
using JuniorBackDotnetProxy.Api.Endpoints;
using JuniorBackDotnetProxy.Api.Middleware;
using JuniorBackDotnetProxy.Domain.Interfaces;
using JuniorBackDotnetProxy.Services;

// Create a builder for the WebApplication
var builder = WebApplication.CreateBuilder(args);


// HTTP Client
// Configure a named HttpClient for connecting to FakeStore API

builder.Services.AddHttpClient("FakeStore", client =>
{
    client.BaseAddress = new Uri("https://fakestoreapi.com/");
});


// Services
// Register the local product service as a singleton
// This allows dependency injection of IProductService throughout the app

builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddScoped<IValidator<ProductDto>, ProductValidator>();

// CORS (Cross-Origin Resource Sharing)
// Allows the frontend apps (Go Live, React/Vite, CRA) to make requests to this API

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://127.0.0.1:5500", // Go Live
                "http://localhost:5173",  // React / Vite
                "http://localhost:3000"   // CRA default
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// Swagger / OpenAPI
// Generates API documentation and Swagger UI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the WebApplication
var app = builder.Build();


// Middleware
// Custom exception handling
// Enables CORS for all requests based on the default policy

app.UseCustomExceptionMiddleware();
app.UseCors();


// Swagger UI
// Enables interactive API documentation at /swagger

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger"; // Swagger UI will be available at /swagger
});


// Map Endpoints
// Registers all product-related endpoints defined in ProductEndpoints

app.MapProductEndpoints();


// Run the API
// Starts the web server

app.Run();
