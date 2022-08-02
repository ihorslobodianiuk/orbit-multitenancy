﻿// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Multitenancy.Context;
using Orbit.Multitenant.Api.Multitenancy.Database;

namespace Orbit.Multitenant.Api.Multitenancy.Middleware
{
    /// <summary>
    /// Used to write the Tenant Header into the <see cref="TenantExecutionContext"/> to 
    /// flow with the async. This uses the <see cref="TenantDbContext"/> to set the Tenant, 
    /// you might find more efficient ways.
    /// </summary>
    public class MultiTenantMiddleware
    {
        /// <summary>
        /// The Header "X-TenantName" the Request contains.
        /// </summary>
        private static readonly string TenantHeaderName = "X-TenantName";

        private readonly RequestDelegate _next;

        public MultiTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TenantDbContext tenantDbContext)
        {
            // Try to get the Tenant Name from the Header:
            if (context.Request.Headers.ContainsKey(TenantHeaderName))
            {
                string tenantName = context.Request.Headers[TenantHeaderName];

                // It's probably OK for the Tenant Name to be empty, which may or may not be valid for your scenario.
                if (!string.IsNullOrWhiteSpace(tenantName))
                {
                    var tenantNameString = tenantName.ToString();

                    var tenant = await tenantDbContext.Tenants
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Name == tenantNameString, context.RequestAborted);

                    if (tenant == null)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Invalid Tenant Name", context.RequestAborted);

                        return;
                    }

                    // We know the Tenant, so set it in the TenantExecutionContext:
                    TenantExecutionContext.SetTenant(tenant);
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
}
