using JuniorBackDotnetProxy.Domain.Entities;

namespace JuniorBackDotnetProxy.Domain.Interfaces;

// Interface for the Product service
public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
}
