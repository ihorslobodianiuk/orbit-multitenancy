using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Tenant.Api.Database;
using Orbit.Tenant.Api.Dto;

namespace Orbit.Tenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly TenantDbContext _dbContext;
    private readonly IMapper _mapper;

    public TenantController(TenantDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var tenants = await _dbContext.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Ok(tenants);
    }
    
    [HttpGet("{tenantId}")]
    public async Task<ActionResult<TenantDto>> GetByName(string tenantName, CancellationToken cancellationToken)
    {
        var tenant = await _dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == tenantName, cancellationToken);

        return Ok(_mapper.Map<TenantDto>(tenant));
    }
    
    [HttpPost]
    public async Task<ActionResult<TenantDto>> Post([FromBody] Models.Tenant tenant, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<TenantDto>(tenant));
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