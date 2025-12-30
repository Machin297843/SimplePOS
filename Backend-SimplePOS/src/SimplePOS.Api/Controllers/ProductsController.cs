using Microsoft.AspNetCore.Mvc;
using SimplePOS.Application.Products;

namespace SimplePOS.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _svc;

    public ProductsController(ProductService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? q,
        [FromQuery] int? groupId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var result = await _svc.ListAsync(q, groupId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _svc.GetByIdAsync(id);
        return Ok(product);
    }
    
    [HttpGet("sku/{sku}")]
    public async Task<IActionResult> GetBySku(string sku)
    {
        var product = await _svc.GetBySkuAsync(sku);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest req)
    {
        var created = await _svc.CreateAsync(req);
        return Created($"/api/products/{created.Id}", created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest req)
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