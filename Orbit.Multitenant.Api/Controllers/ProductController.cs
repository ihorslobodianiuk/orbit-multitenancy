using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Database;
using Orbit.Multitenant.Api.Dto;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly OrbitDbContext _orbitDbContext;
    private readonly IMapper _mapper;

    public ProductController(OrbitDbContext orbitDbContext,
        IMapper mapper)
    {
        this._orbitDbContext = orbitDbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var products = _orbitDbContext.Products.AsNoTracking();

        var productDtos = _mapper.ProjectTo<List<ProductDto>>(products);

        return Ok(productDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var product = await _orbitDbContext.Products.FindAsync(id);

        if(product == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ProductDto>(product));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductDto productDto, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(productDto);

        await _orbitDbContext.AddAsync(product, cancellationToken);
        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = product.Id }, _mapper.Map<ProductDto>(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProductDto productDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(productDto);

        _orbitDbContext.Products.Update(product);
        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
        

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var product = await _orbitDbContext.Products.FindAsync(id);

        if(product == null)
        {
            return NotFound();
        }

        _orbitDbContext.Products.Remove(product);

        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}