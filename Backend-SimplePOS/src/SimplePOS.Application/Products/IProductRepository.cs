using SimplePOS.Domain.Entities;

namespace SimplePOS.Application.Products;

public interface IProductRepository
{
    void Add(Product product);
    Task<List<Product>> ListAsync(string? q, int? groupId, int? page, int? pageSize);
    Task<bool> ExistsBySkuAsync(string sku, int? excludedId = null);
    Task<bool> ExistsByNameAsync(string name, int? excludedId = null);
    Task<Product?> GetTrackedByIdAsync(int id);
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetBySkuAsync(string sku);
    void Remove(Product product); 
    Task SaveChangesAsync();
    
}