using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Orbit.Application.Api.Middleware;

namespace Orbit.Application.Api.Infrastructure;

public class FeatureToggleFilter : ActionFilterAttribute
{
    public string Feature { get; }

    public FeatureToggleFilter(string feature)
    {
        Feature = feature;
    }
   
    // Pull the user ID on each request
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (string.IsNullOrEmpty(Feature))
            return;
        
        var domainContext = context.HttpContext.RequestServices.GetService<IDomainContextInfo>();
        if (domainContext != null && !domainContext.Features.Any(f => f.Name == Feature && f.Enabled))
            throw new Exception($"Feature {Feature} is not enabled");
    }
}