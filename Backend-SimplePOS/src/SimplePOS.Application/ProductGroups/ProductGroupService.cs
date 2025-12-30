using SimplePOS.Domain.Entities;
using SimplePOS.Application.Common.Errors;

namespace SimplePOS.Application.ProductGroups;

public class ProductGroupService
{
    private readonly IProductGroupRepository _productGroups;

    public ProductGroupService(IProductGroupRepository productGroups)
    {
        _productGroups = productGroups;
    }

    public async Task<ProductGroupResponse> CreateAsync(CreateProductGroupRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            throw new ArgumentException("Name is required.");

        var trimmedName = req.Name.Trim();

        if (await _productGroups.ExistsByNameAsync(trimmedName))
            throw new ConflictException("Product group name already exists.");

        var group = new ProductGroup(trimmedName);
        _productGroups.Add(group);

        await _productGroups.SaveChangesAsync();

        return new ProductGroupResponse(group.Id, group.Name);
    }

    public async Task<List<ProductGroupResponse>> ListAsync()
    {
        var groups = await _productGroups.ListAsync();
        return groups.Select(g => new ProductGroupResponse(g.Id, g.Name)).ToList();
    }

    public async Task<ProductGroupResponse> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Id is invalid.");

        var group = await _productGroups.GetTrackedByIdAsync(id);
        if (group is null)
            throw new NotFoundException("Product group not found.");

        return new ProductGroupResponse(group.Id, group.Name);
    }
    public async Task<ProductGroupResponse> UpdateAsync(int id, UpdateProductGroupRequest req)
    {
        if (id <= 0) throw new ArgumentException("Id is invalid.");

        if (string.IsNullOrWhiteSpace(req.Name))
            throw new ArgumentException("Name is required.");
 
        var name = req.Name.Trim();

        if (await _productGroups.ExistsByNameAsync(name, id))
            throw new ConflictException("Product group name already exists.");

        var group = await _productGroups.GetTrackedByIdAsync(id);
        if (group is null)
            throw new NotFoundException("Product group not found.");

        group.SetName(name);
        await _productGroups.SaveChangesAsync();

        return new ProductGroupResponse(group.Id, group.Name);
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Id is invalid.");
        
        var group = await _productGroups.GetTrackedByIdAsync(id);
        if (group is null)
            throw new NotFoundException("Product group not found.");

        _productGroups.Remove(group);
        await _productGroups.SaveChangesAsync();
    }
}