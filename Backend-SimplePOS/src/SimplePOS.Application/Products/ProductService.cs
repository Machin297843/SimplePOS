using SimplePOS.Application.ProductGroups;
using SimplePOS.Domain.Entities;
using SimplePOS.Application.Common.Errors;

namespace SimplePOS.Application.Products;

public class ProductService
{
    private readonly IProductRepository _products;
    private readonly IProductGroupRepository _groups;

    public ProductService(IProductRepository products, IProductGroupRepository groups)
    {
        _products = products;
        _groups = groups;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Sku)) throw new ArgumentException("Sku is required.");
        if (string.IsNullOrWhiteSpace(req.Name)) throw new ArgumentException("Name is required.");
        if (req.Price < 0) throw new ArgumentException("Price cannot be negative.");
        if (req.ProductGroupId <= 0) throw new ArgumentException("ProductGroupId is invalid.");

        var sku = req.Sku.Trim();

        if (!await _groups.ExistsAsync(req.ProductGroupId))
            throw new ArgumentException("Product group does not exist.");

        if (await _products.ExistsBySkuAsync(sku))
            throw new ConflictException("Sku already exists.");

        var product = new Product(sku, req.Name.Trim(), req.Price, req.ProductGroupId);

        _products.Add(product);
        await _products.SaveChangesAsync();

        return new ProductResponse(product.Id, product.Sku, product.Name, product.Price, product.ProductGroupId);
    }

    public async Task<List<ProductResponse>> ListAsync()
    {
        var products = await _products.ListAsync();
        return products.Select(p => new ProductResponse(p.Id, p.Sku, p.Name, p.Price, p.ProductGroupId)).ToList();
    }
}