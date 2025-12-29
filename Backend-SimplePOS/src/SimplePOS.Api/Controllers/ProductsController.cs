using Microsoft.AspNetCore.Mvc;
using SimplePOS.Application.Products;

namespace SimplePOS.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest req,
        [FromServices] ProductService svc)
    {
        var created = await svc.CreateAsync(req);
        return Created($"/api/products/{created.Id}", created);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromServices] ProductService svc)
    {
        var items = await svc.ListAsync();
        return Ok(items);
    }
}