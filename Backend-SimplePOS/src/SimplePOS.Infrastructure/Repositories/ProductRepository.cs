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

    public Task<List<Product>> ListAsync() =>
        _db.Products.AsNoTracking().OrderBy(x => x.Id).ToListAsync();

    public Task<bool> ExistsBySkuAsync(string sku) =>
        _db.Products.AsNoTracking().AnyAsync(x => x.Sku == sku);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}