using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Database;

namespace Orbit.Multitenant.Api.Middleware;

public class MultiTenantMiddleware
{
    private static readonly string TenantHeaderName = "X-TenantName";

    private readonly RequestDelegate _next;

    public MultiTenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, OrbitDbContext dbContext, IDomainContextInfo domainContextInfo)
    {
        if (context.Request.Headers.ContainsKey(TenantHeaderName))
        {
            string tenantName = context.Request.Headers[TenantHeaderName];
            
            if (!string.IsNullOrWhiteSpace(tenantName))
            {
                var tenantNameString = tenantName;

                var tenant = await dbContext.Tenants
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == tenantNameString, context.RequestAborted);

                if (tenant == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid Tenant Name", context.RequestAborted);

                    return;
                }

                domainContextInfo.TenantId = new Guid(tenantName);
            }
        }

        await _next(context);
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MultiTenantMiddleware>();
    }
}