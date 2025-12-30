using Microsoft.AspNetCore.Mvc;
using SimplePOS.Application.ProductGroups;

namespace SimplePOS.Api.Controllers;

[ApiController]
[Route("api/product-groups")]
public class ProductGroupsController : ControllerBase
{
    private readonly ProductGroupService _svc;

    public ProductGroupsController(ProductGroupService svc)
    {
        _svc = svc;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductGroupRequest req)
    {
        var created = await _svc.CreateAsync(req);
        return Created($"/api/product-groups/{created.Id}", created);
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var items = await _svc.ListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _svc.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductGroupRequest req)
    {
        var updated = await _svc.UpdateAsync(id, req);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}