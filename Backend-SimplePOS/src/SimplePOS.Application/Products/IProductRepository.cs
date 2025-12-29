using SimplePOS.Domain.Entities;

namespace SimplePOS.Application.Products;

public interface IProductRepository
{
    void Add(Product product);
    Task<List<Product>> ListAsync();
    Task<bool> ExistsBySkuAsync(string sku);
    Task SaveChangesAsync();
}