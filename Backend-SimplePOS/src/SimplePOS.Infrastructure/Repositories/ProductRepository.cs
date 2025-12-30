using Microsoft.EntityFrameworkCore;
using SimplePOS.Application.Products;
using SimplePOS.Domain.Entities;
using SimplePOS.Infrastructure.Data;

namespace SimplePOS.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public void Add(Product product) => _db.Products.Add(product);

    public void Remove(Product product) => _db.Products.Remove(product);

    public async Task<List<Product>> ListAsync(string? q, int? groupId, int? page, int? pageSize)
    {
        var query = _db.Products.AsNoTracking().AsQueryable();

        if (groupId.HasValue && groupId.Value > 0)
            query = query.Where(x => x.ProductGroupId == groupId.Value);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            var pattern = $"%{term}%";

            query = query.Where(x =>
                EF.Functions.ILike(x.Name, pattern) ||
                EF.Functions.ILike(x.Sku, pattern));
        }

        query = query.OrderBy(x => x.Id);

        // paging
        var p = page.GetValueOrDefault(1);
        var ps = pageSize.GetValueOrDefault(50);

        if (p < 1) p = 1;
        if (ps < 1) ps = 50;
        if (ps > 200) ps = 200;

        query = query.Skip((p - 1) * ps).Take(ps);

        return await query.ToListAsync();
    }

    public Task<bool> ExistsBySkuAsync(string sku, int? excludedId = null)
    {
        var query = _db.Products.AsNoTracking().Where(x => x.Sku == sku);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
    }

    public Task<bool> ExistsByNameAsync(string name, int? excludedId = null)
    {
        var query = _db.Products.AsNoTracking().Where(x => x.Name == name);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
    }
    public Task<Product?> GetTrackedByIdAsync(int id) =>
        _db.Products.FirstOrDefaultAsync(x => x.Id == id);
        
    public Task<Product?> GetByIdAsync(int id) =>
        _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public Task<Product?> GetBySkuAsync(string sku) =>
        _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Sku == sku);
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}