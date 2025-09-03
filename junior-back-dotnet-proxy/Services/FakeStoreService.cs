using junior_back_dotnet_proxy.Utils;
using JuniorBackDotnetProxy.Domain.Entities;
using JuniorBackDotnetProxy.Domain.Interfaces;
using System.Net.Http.Json;

namespace JuniorBackDotnetProxy.Services;


/// Service to interact with the FakeStore API and map its products
/// to the internal Product entity used in the application.

public class FakeStoreService
{
    private readonly HttpClient _http;

  
    /// Constructor that uses IHttpClientFactory to create a named HttpClient
    /// for connecting to the FakeStore API.
   
    /// <param name="factory">HTTP client factory</param>
    public FakeStoreService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("FakeStore");
    }

  
    /// Fetches all products from the FakeStore API and maps them
    /// to the internal Product entity.
 
    /// <returns>Enumerable of Product objects</returns>
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        // Call FakeStore API and deserialize JSON response into a list of FakeStoreProduct
        var fakeProducts = await _http.GetFromJsonAsync<List<FakeStoreProduct>>("products");

        // If the API returns null, return an empty list
        if (fakeProducts == null) return Enumerable.Empty<Product>();

        // Map each FakeStoreProduct to internal Product entity
        return fakeProducts.Select(fake => new Product
        {
            Id = fake.Id,
            Name = fake.Title,
            Brand = "Unknown", // FakeStore API does not provide brand info
            Price = fake.Price,
            Stock = 0,         // FakeStore API does not provide stock info
            Image = fake.Image,
            Category = fake.Category,
            CreatedAt = DateTime.Now // Set current timestamp as created date
        });
    }
}
