using FluentValidation;
using junior_back_dotnet_proxy.Utils;
using JuniorBackDotnetProxy.Api.Dtos;
using JuniorBackDotnetProxy.Domain.Entities;
using JuniorBackDotnetProxy.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace JuniorBackDotnetProxy.Api.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            
            // GET /api/products
            // Fetch products from FakeStore API with optional filters and pagination
            // Optional params: query (search), category, page, pageSize
            
            app.MapGet("/api/products",
                async (IHttpClientFactory httpClientFactory, string? query, string? category, int? page, int? pageSize) =>
                {
                    int currentPage = page ?? 1;
                    int currentPageSize = pageSize ?? 10;

                    // Validate pagination parameters
                    if (currentPage <= 0 || currentPageSize <= 0)
                        throw new ArgumentException("page and pageSize must be greater than 0.");

                    // Create HTTP client for FakeStore
                    var client = httpClientFactory.CreateClient("FakeStore");
                    var resp = await client.GetAsync("products", HttpCompletionOption.ResponseHeadersRead);
                    if (!resp.IsSuccessStatusCode)
                        throw new HttpRequestException($"FakeStoreAPI returned {(int)resp.StatusCode} for /products.");

                    // Deserialize JSON response
                    var products = await resp.Content.ReadFromJsonAsync<IEnumerable<FakeStoreProduct>>(
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (products is null || !products.Any())
                        throw new KeyNotFoundException("No products found.");

                    // Map external products to DTO
                    var productsDto = products.Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Title,
                        Brand = "Unknown",
                        Price = p.Price,
                        Stock = 0,
                        Image = p.Image,
                        Category = p.Category
                    });

                    // Filtering
                    IEnumerable<ProductDto> filtered = productsDto;

                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        filtered = filtered.Where(p =>
                            p.Name.Contains(query, StringComparison.Ordinal) ||
                            p.Category.Contains(query, StringComparison.Ordinal)
                        );
                    }

                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        filtered = filtered.Where(p =>
                            string.Equals(p.Category, category, StringComparison.Ordinal)
                        );
                    }


                    //  Pagination
                    int total = filtered.Count();
                    var items = filtered.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);

                    // Return paginated response
                    var pageDto = new PagedResponseDto<ProductDto>
                    {
                        Items = items,
                        Page = currentPage,
                        PageSize = currentPageSize,
                        Total = total
                    };

                    return Results.Ok(pageDto);
                });


            // GET /api/products/{id}
            // Fetch a single product by its ID from FakeStore API

            app.MapGet("/api/products/{id:int}",
                async (int id, IHttpClientFactory httpClientFactory, CancellationToken cancellationToken) =>
                {
                    if (id <= 0)
                        return Results.BadRequest("The id must be greater than 0.");

                    using var client = httpClientFactory.CreateClient("FakeStore");

                    try
                    {
                        using var response = await client.GetAsync($"products/{id}",
                            HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                        return response.StatusCode switch
                        {
                            HttpStatusCode.NotFound => Results.NotFound($"Product with ID {id} not found."),
                            HttpStatusCode.OK => await ProcessSuccessResponse(response, cancellationToken),
                            _ => Results.StatusCode(500) // Internal server error for other failures
                        };
                    }
                    catch (HttpRequestException)
                    {
                        return Results.StatusCode(503); // Service unavailable
                    }
                    catch (TaskCanceledException)
                    {
                        return Results.StatusCode(408); // Request timeout
                    }
                });

            static async Task<IResult> ProcessSuccessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
            {
                var product = await response.Content.ReadFromJsonAsync<FakeStoreProduct>(
                    JsonSerializerOptions.Default, cancellationToken);

                if (product is null)
                    return Results.NotFound("Product data could not be parsed.");

                var dto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Title,
                    Brand = "Unknown",
                    Price = product.Price,
                    Stock = 0,
                    Image = product.Image,
                    Category = product.Category
                };

                return Results.Ok(dto);
            }


            // GET /api/categories
            // Fetch all product categories from FakeStore API

            app.MapGet("/api/categories",
                async (IHttpClientFactory httpClientFactory) =>
                {
                    var client = httpClientFactory.CreateClient("FakeStore");
                    var resp = await client.GetAsync("products/categories", HttpCompletionOption.ResponseHeadersRead);
                    if (!resp.IsSuccessStatusCode)
                        throw new HttpRequestException($"FakeStoreAPI returned {(int)resp.StatusCode} for /products/categories.");

                    var categories = await resp.Content.ReadFromJsonAsync<string[]>(
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return Results.Ok(categories ?? Array.Empty<string>());
                });

       
            // GET /api/local-products
            // Fetch all products from in-memory local storage (IProductService)
      
            app.MapGet("/api/local-products", async (IProductService service) => Results.Ok(await service.GetProductsAsync()));

           
            // GET /api/local-products/{id}
            // Fetch a single product from local storage by ID
   
            app.MapGet("/api/local-products/{id:int}", async (int id, IProductService service) =>
            {
                if (id <= 0) throw new ArgumentException("The id must be greater than 0.");
                var prod = await service.GetProductByIdAsync(id);
                if (prod is null) throw new KeyNotFoundException($"Local product with ID {id} not found.");
                return Results.Ok(prod);
            });

            // POST /api/products
            // Create a new product in local storage
            // Validates all fields before saving


            app.MapPost("/api/products", async (ProductDto dto, IProductService service, IValidator<ProductDto> validator) =>
            {
                // Validación del DTO
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                // Crear el objeto Product directamente
                var product = new Product
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Brand = dto.Brand,
                    Price = dto.Price,
                    // Agrega aquí cualquier otra propiedad que tenga Product
                };

                var created = await service.CreateProductAsync(product);

                return Results.Created($"/api/local-products/{created.Id}", new
                {
                    message = "Product created successfully.",
                    data = created
                });
            });





            // DELETE /api/products/{id}
            // Delete a product from local storage by ID

            app.MapDelete("/api/products/{id:int}", async (int id, IProductService service) =>
            {
                if (id <= 0) throw new ArgumentException("The id must be greater than 0.");

                var deleted = await service.DeleteProductAsync(id);
                if (!deleted)
                    throw new KeyNotFoundException($"Product with id {id} not found.");

                return Results.NoContent();
            });
        }
    }
}
