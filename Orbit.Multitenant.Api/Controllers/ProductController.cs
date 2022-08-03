using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Converters;
using Orbit.Multitenant.Api.Database;
using Orbit.Multitenant.Api.Dto;

namespace Orbit.Multitenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly OrbitDbContext _orbitDbContext;

    public ProductController(OrbitDbContext orbitDbContext)
    {
        this._orbitDbContext = orbitDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var customers = await _orbitDbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var customerDtos = customers
            .Select(ProductConverter.ToDto)
            .ToList();

        return Ok(customerDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var customer = await _orbitDbContext.Products.FindAsync(id);

        if(customer == null)
        {
            return NotFound();
        }

        var product = ProductConverter.ToDto(customer);

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductDto productDto, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = ProductConverter.ToModel(productDto);

        await _orbitDbContext.AddAsync(product, cancellationToken);
        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = product.Id }, ProductConverter.ToDto(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProductDto productDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = ProductConverter.ToModel(productDto);

        _orbitDbContext.Products.Update(customer);
        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
        

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var customer = await _orbitDbContext.Products.FindAsync(id);

        if(customer == null)
        {
            return NotFound();
        }

        _orbitDbContext.Products.Remove(customer);

        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}