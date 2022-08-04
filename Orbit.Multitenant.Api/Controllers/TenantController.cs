using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Database;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly OrbitDbContext _orbitDbContext;

    public TenantController(OrbitDbContext orbitDbContext)
    {
        this._orbitDbContext = orbitDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var tenants = await _orbitDbContext.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Ok(tenants);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Tenant tenant, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _orbitDbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return Ok(tenant);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var tenant = await _orbitDbContext.Tenants.FindAsync(id);

        if(tenant == null)
        {
            return NotFound();
        }

        _orbitDbContext.Tenants.Remove(tenant);

        await _orbitDbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}