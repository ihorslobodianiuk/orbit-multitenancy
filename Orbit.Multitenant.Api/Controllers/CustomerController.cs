using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Converters;
using Orbit.Multitenant.Api.Database;
using Orbit.Multitenant.Api.Dto;

namespace Orbit.Multitenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly OrbitDbContext _orbitDbContext;

    public CustomerController(OrbitDbContext orbitDbContext)
    {
        this._orbitDbContext = orbitDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var customers = await _orbitDbContext.Customers
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var customerDtos = customers
            .Select(CustomerConverter.ToDto)
            .ToList();

        return Ok(customerDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var customer = await _orbitDbContext.Customers.FindAsync(id);

        if(customer == null)
        {
            return NotFound();
        }

        var customerDto = CustomerConverter.ToDto(customer);

        return Ok(customerDto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CustomerDto customerDto, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = CustomerConverter.ToModel(customerDto);

        await _orbitDbContext.AddAsync(customer, cancellationToken);
        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = customer.Id }, CustomerConverter.ToDto(customer));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CustomerDto customerDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = CustomerConverter.ToModel(customerDto);

        _orbitDbContext.Customers.Update(customer);
        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
        

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var customer = await _orbitDbContext.Customers.FindAsync(id);

        if(customer == null)
        {
            return NotFound();
        }

        _orbitDbContext.Customers.Remove(customer);

        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}