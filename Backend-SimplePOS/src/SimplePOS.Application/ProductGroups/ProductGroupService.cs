using SimplePOS.Domain.Entities;
using SimplePOS.Application.Common.Errors;

namespace SimplePOS.Application.ProductGroups;

public class ProductGroupService
{
    private readonly IProductGroupRepository _repo;

    public ProductGroupService(IProductGroupRepository repo)
    {
        _repo = repo;
    }

    public async Task<ProductGroupResponse> CreateAsync(CreateProductGroupRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            throw new ArgumentException("Name is required.");

        var group = new ProductGroup(req.Name.Trim());

        _repo.Add(group);
        await _repo.SaveChangesAsync();

        return new ProductGroupResponse(group.Id, group.Name);
    }

    public async Task<List<ProductGroupResponse>> ListAsync()
    {
        var groups = await _repo.ListAsync();
        return groups.Select(g => new ProductGroupResponse(g.Id, g.Name)).ToList();
    }

    public async Task<ProductGroupResponse> UpdateAsync(int id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        var group = await _repo.GetTrackedByIdAsync(id);
        if (group is null)
            throw new NotFoundException("Product group not found.");

        group.SetName(name.Trim());
        await _repo.SaveChangesAsync();

        return new ProductGroupResponse(group.Id, group.Name);
    }

    public async Task DeleteAsync(int id)
    {
        var group = await _repo.GetTrackedByIdAsync(id);
        if (group is null)
            throw new NotFoundException("Product group not found.");

        _repo.Remove(group);
        await _repo.SaveChangesAsync();
    }
}