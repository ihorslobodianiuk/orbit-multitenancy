using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Tenant.Api.Database;

namespace Orbit.Tenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly TenantDbContext _dbContext;

    public TenantController(TenantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var tenants = await _dbContext.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Ok(tenants);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Models.Tenant tenant, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(tenant);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var tenant = await _dbContext.Tenants.FindAsync(id);

        if(tenant == null)
        {
            return NotFound();
        }

        _dbContext.Tenants.Remove(tenant);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}