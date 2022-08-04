using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Tenant.Api.Database;
using Orbit.Tenant.Api.Dto;

namespace Orbit.Tenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class FeatureController : ControllerBase
{
    private readonly TenantDbContext _dbContext;
    private readonly IMapper _mapper;

    public FeatureController(TenantDbContext dbContext,
        IMapper mapper)
    {
        this._dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var features = await _dbContext.Features.ToListAsync(cancellationToken);

        var featureDtos = features
            .Select(p => _mapper.Map<FeatureDto>(p))
            .ToList();

        return Ok(featureDtos);
    }

    [HttpPost]
    public async Task<IActionResult> Post(int featureId, bool enabled, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var feature = await _dbContext.Features.SingleOrDefaultAsync(f => f.FeatureId == featureId, cancellationToken);
        if (feature != null && enabled != feature.Enabled)
        {
            // feature.Toggle(_domainContextInfo.TenantId!.Value);
            _dbContext.Features.Update(feature);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return NoContent();
    }
}