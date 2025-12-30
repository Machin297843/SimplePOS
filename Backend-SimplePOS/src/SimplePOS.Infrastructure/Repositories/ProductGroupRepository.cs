using Microsoft.EntityFrameworkCore;
using SimplePOS.Application.ProductGroups;
using SimplePOS.Domain.Entities;
using SimplePOS.Infrastructure.Data;

namespace SimplePOS.Infrastructure.Repositories;

public class ProductGroupRepository : IProductGroupRepository
{
    private readonly AppDbContext _db;
    public ProductGroupRepository(AppDbContext db) => _db = db;

    public void Add(ProductGroup group) => _db.ProductGroups.Add(group);

    public Task<List<ProductGroup>> ListAsync() =>
        _db.ProductGroups.AsNoTracking().OrderBy(x => x.Id).ToListAsync();

    public Task<bool> ExistsByIdAsync(int id, int? excludedId = null)
    {
        var query = _db.ProductGroups.AsNoTracking().Where(x => x.Id == id);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
    }
   public Task<bool> ExistsByNameAsync(string name, int? excludedId = null)
    {
        var query = _db.ProductGroups.AsNoTracking().Where(x => x.Name == name);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
    }
    public Task<ProductGroup?> GetTrackedByIdAsync(int id) =>
        _db.ProductGroups.FirstOrDefaultAsync(x => x.Id == id);

    public void Remove(ProductGroup group) => _db.ProductGroups.Remove(group);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}