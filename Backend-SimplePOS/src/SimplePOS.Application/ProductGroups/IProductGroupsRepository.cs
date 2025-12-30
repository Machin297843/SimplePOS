using SimplePOS.Domain.Entities;

namespace SimplePOS.Application.ProductGroups;

public interface IProductGroupRepository
{
    void Add(ProductGroup group);
    Task<List<ProductGroup>> ListAsync();
    Task<bool> ExistsByIdAsync(int id, int? excludedId = null);
    Task<bool> ExistsByNameAsync(string name, int? excludedId = null);
    Task<ProductGroup?> GetTrackedByIdAsync(int id);
    void Remove(ProductGroup group);
    Task SaveChangesAsync();
}