using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.TenantManagement.Api.Database;
using Orbit.TenantManagement.Api.Dto;
using Orbit.TenantManagement.Api.Models;

namespace Orbit.TenantManagement.Api.Controllers;

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
    
    [HttpGet("{tenantName}")]
    public async Task<ActionResult<TenantDto>> GetByName(string tenantName, CancellationToken cancellationToken)
    {
        var tenant = await _dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == tenantName, cancellationToken);

        return Ok(_mapper.Map<TenantDto>(tenant));
    }
    
    [HttpPost]
    public async Task<ActionResult<TenantDto>> Post([FromBody] TenantPostDto tenant, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newTenant = await _dbContext.Tenants.AddAsync(_mapper.Map<Tenant>(tenant), cancellationToken);
        var username = newTenant.Entity.TenantId.ToString();
        await _dbContext.Database.ExecuteSqlRawAsync($"create user \"{username}\" with password '123';", cancellationToken: cancellationToken);
        await _dbContext.Database.ExecuteSqlRawAsync($"GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO \"{username}\";", cancellationToken: cancellationToken);
        await _dbContext.Database.ExecuteSqlRawAsync($"grant delete, insert, select, update on product to \"{username}\";", cancellationToken: cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<TenantDto>(newTenant.Entity));
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