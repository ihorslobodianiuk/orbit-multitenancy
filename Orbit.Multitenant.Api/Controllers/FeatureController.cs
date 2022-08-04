using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Database;
using Orbit.Multitenant.Api.Dto;
using Orbit.Multitenant.Api.Middleware;

namespace Orbit.Multitenant.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class FeatureController : ControllerBase
{
    private readonly OrbitDbContext _orbitDbContext;
    private readonly IDomainContextInfo _domainContextInfo;
    private readonly IMapper _mapper;

    public FeatureController(OrbitDbContext orbitDbContext,
        IDomainContextInfo domainContextInfo,
        IMapper mapper)
    {
        this._orbitDbContext = orbitDbContext;
        _domainContextInfo = domainContextInfo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var features = await _orbitDbContext.Features.ToListAsync(cancellationToken);

        var featureDtos = features
            .Select(p => _mapper.Map<FeatureDto>(p))
            .ToList();

        return Ok(featureDtos);
    }

    [HttpPut("{featureId}/{enabled}")]
    public async Task<IActionResult> Put(int featureId, bool enabled, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var feature = await _orbitDbContext.Features.SingleOrDefaultAsync(f => f.FeatureId == featureId, cancellationToken);
        if (feature != null && enabled != feature.Enabled)
        {
            feature.Toggle(_domainContextInfo.TenantId!.Value);
            _orbitDbContext.Features.Update(feature);
            await _orbitDbContext.SaveChangesAsync(cancellationToken);
        }

        return NoContent();
    }
}