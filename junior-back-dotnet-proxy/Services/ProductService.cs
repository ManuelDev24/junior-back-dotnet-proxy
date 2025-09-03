using JuniorBackDotnetProxy.Domain.Entities;
using JuniorBackDotnetProxy.Domain.Interfaces;
using System.Linq;

namespace JuniorBackDotnetProxy.Services;

/// Service that manages products in memory (no database involved).
/// Implements the IProductService interface.
public class ProductService : IProductService
{
    // In-memory list of products as mock data
    private readonly List<Product> _products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Brand = "Dell",
            Price = 1200,
            Stock = 10,
            Image = "laptop.jpg",
            Category = "Electronics",
            CreatedAt = DateTime.Now
        },
        new Product
        {
            Id = 2,
            Name = "Phone",
            Brand = "Samsung",
            Price = 800,
            Stock = 15,
            Image = "phone.jpg",
            Category = "Electronics",
            CreatedAt = DateTime.Now
        }
    };

    /// Retrieves all products in memory.
    /// <returns>An enumerable of Product objects.</returns>
    public Task<IEnumerable<Product>> GetProductsAsync() =>
        Task.FromResult(_products.AsEnumerable());

    /// Retrieves a single product by its ID.
    /// Returns null if not found.
    /// <param name="id">Product ID to search for</param>
    /// <returns>Product object or null</returns>
    public Task<Product?> GetProductByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    
    /// Creates a new product and adds it to the in-memory list.
    /// Auto-increments ID and sets CreatedAt timestamp.
    /// If Brand is empty, sets it to "Unknown".
  
    /// <param name="product">Product object to create</param>
    /// <returns>The created Product object</returns>
    public Task<Product> CreateProductAsync(Product product)
    {
        // Auto-increment ID based on the current max ID
        product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
        product.CreatedAt = DateTime.Now;

        // Set default brand if missing
        if (string.IsNullOrWhiteSpace(product.Brand))
            product.Brand = "Unknown";

        _products.Add(product);
        return Task.FromResult(product);
    }

    
    /// Deletes a product by its ID.
    /// Returns false if the product is not found.
  
    /// <param name="id">ID of the product to delete</param>
    /// <returns>True if deleted, false if not found</returns>
    public Task<bool> DeleteProductAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return Task.FromResult(false); // Product not found

        _products.Remove(product);
        return Task.FromResult(true); // Product deleted successfully
    }
}
