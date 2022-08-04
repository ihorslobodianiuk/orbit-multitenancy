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

    [HttpGet("{tenantId}")]
    public async Task<IActionResult> GetTenantFeatures(Guid tenantId)
    {
        return Ok(await _dbContext.TenantFeatures.Include(tf => tf.Feature)
            .Where(tf => tf.TenantId == tenantId)
            .Select(tf => _mapper.Map<TenantFeatureDto>(tf)).ToListAsync());
    }
    
    [HttpPost("tenant")]
    public async Task<IActionResult> PostTenantFeature([FromBody] TenantFeaturePostDto tenantFeaturePost, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var feature = await _dbContext.TenantFeatures.AddAsync(_mapper.Map<TenantFeature>(tenantFeaturePost), cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Ok(_mapper.Map<TenantFeaturePostDto>(feature.Entity));
    }
}