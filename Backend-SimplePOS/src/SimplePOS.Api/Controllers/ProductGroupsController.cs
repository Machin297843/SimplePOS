using Microsoft.AspNetCore.Mvc;
using SimplePOS.Application.ProductGroups;

namespace SimplePOS.Api.Controllers;

[ApiController]
[Route("api/product-groups")]
public class ProductGroupsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductGroupRequest req,
        [FromServices] ProductGroupService svc)
    {
        var created = await svc.CreateAsync(req);
        return Created($"/api/product-groups/{created.Id}", created);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromServices] ProductGroupService svc)
    {
        var items = await svc.ListAsync();
        return Ok(items);
    }
}