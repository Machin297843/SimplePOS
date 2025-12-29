using SimplePOS.Domain.Entities;

namespace SimplePOS.Application.ProductGroups;

public interface IProductGroupRepository
{
    void Add(ProductGroup group);
    Task<List<ProductGroup>> ListAsync();
    Task<bool> ExistsAsync(int id);

    Task<ProductGroup?> GetTrackedByIdAsync(int id);
    void Remove(ProductGroup group);

    Task SaveChangesAsync();
}