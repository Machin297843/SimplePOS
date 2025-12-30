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
        var name = req.Name.Trim();

        if (!await _groups.ExistsByIdAsync(req.ProductGroupId))
            throw new ArgumentException("Product group does not exist.");

        if (await _products.ExistsBySkuAsync(sku))
            throw new ConflictException("Sku already exists.");

        if (await _products.ExistsByNameAsync(name))
            throw new ConflictException("Name already exists.");

        var product = new Product(sku, name, req.Price, req.ProductGroupId);

        _products.Add(product);
        await _products.SaveChangesAsync();

        return new ProductResponse(product.Id, product.Sku, product.Name, product.Price, product.ProductGroupId);
    }

    public async Task<List<ProductResponse>> ListAsync(string? q, int? groupId, int? page, int? pageSize)
    {
        var products = await _products.ListAsync(q, groupId, page, pageSize);
        return products.Select(p => new ProductResponse(p.Id, p.Sku, p.Name, p.Price, p.ProductGroupId)).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Id is invalid.");

        var product = await _products.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException("Product not found.");

        return new ProductResponse(product.Id, product.Sku, product.Name, product.Price, product.ProductGroupId);
    }
    public async Task<ProductResponse> GetBySkuAsync(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("Sku is required.");
        var trimmedSku = sku.Trim();
        var product = await _products.GetBySkuAsync(trimmedSku);
        if (product == null)
            throw new NotFoundException("Product not found.");

        return new ProductResponse(product.Id, product.Sku, product.Name, product.Price, product.ProductGroupId);
    }
    public async Task<ProductResponse> UpdateAsync(int id, UpdateProductRequest req)
    {
        if (id <= 0) throw new ArgumentException("Id is invalid.");
        if (string.IsNullOrWhiteSpace(req.Sku)) throw new ArgumentException("Sku is required.");
        if (string.IsNullOrWhiteSpace(req.Name)) throw new ArgumentException("Name is required.");
        if (req.Price < 0) throw new ArgumentException("Price cannot be negative.");
        if (req.ProductGroupId <= 0) throw new ArgumentException("Product group invalid");

        var product = await _products.GetTrackedByIdAsync(id);
        if (product == null) throw new NotFoundException("Product not found.");

        var sku = req.Sku.Trim();
        var name = req.Name.Trim();

        if (!await _groups.ExistsByIdAsync(req.ProductGroupId))
            throw new ArgumentException("Product group does not exist.");

        if (await _products.ExistsBySkuAsync(sku, excludedId: id))
            throw new ConflictException("Sku already exists.");

        if (await _products.ExistsByNameAsync(name, excludedId: id))
            throw new ConflictException("Name already exists.");

        product.Update(sku, name, req.Price, req.ProductGroupId);
        await _products.SaveChangesAsync();

        return new ProductResponse(product.Id, product.Sku, product.Name, product.Price, product.ProductGroupId);
    }
    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Id is invalid.");

        var product = await _products.GetTrackedByIdAsync(id);
        if (product == null)
            throw new NotFoundException("Product not found.");

        _products.Remove(product);
        await _products.SaveChangesAsync();
    }
}