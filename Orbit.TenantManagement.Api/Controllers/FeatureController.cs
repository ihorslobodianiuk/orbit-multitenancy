using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.TenantManagement.Api.Database;
using Orbit.TenantManagement.Api.Dto;
using Orbit.TenantManagement.Api.Models;

namespace Orbit.TenantManagement.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class FeatureController : ControllerBase
{
    private readonly TenantDbContext _dbContext;
    private readonly IMapper _mapper;

    public FeatureController(TenantDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
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
    public async Task<IActionResult> Post([FromBody] FeatureDto feature, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var featureEntity = await _dbContext.Features.SingleOrDefaultAsync(f => f.FeatureId == feature.FeatureId, cancellationToken);
        if (featureEntity != null)
        {
            _mapper.Map(feature, featureEntity);
            _dbContext.Features.Update(featureEntity);
        }
        else
        {
            await _dbContext.Features.AddAsync(_mapper.Map<Feature>(feature), cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(_mapper.Map<FeatureDto>(feature));
    }
    
    [HttpPost("tenant")]
    public async Task<IActionResult> PostTenantFeature([FromBody] TenantFeatureDto feature, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var featureEntity = await _dbContext.Features.SingleOrDefaultAsync(f => f.FeatureId == feature.FeatureId, cancellationToken);
        // if (featureEntity != null)
        // {
        //     _mapper.Map<TenantFeature>()
        // }
        // if (feature != null && enabled != feature.Enabled)
        // {
        //     // feature.Toggle(_domainContextInfo.TenantId!.Value);
        //     _dbContext.Features.Update(feature);
        //     await _dbContext.SaveChangesAsync(cancellationToken);
        // }

        return NoContent();
    }
}