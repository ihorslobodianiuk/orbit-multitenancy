using Microsoft.EntityFrameworkCore;
using Orbit.Tenant.Api.Database.Mappings;
using Orbit.Tenant.Api.Models;

namespace Orbit.Tenant.Api.Database;

public class TenantDbContext : DbContext
{
    public DbSet<Models.Tenant> Tenants { get; set; }
    public DbSet<Feature> Features { get; set; }

    public TenantDbContext(DbContextOptions<TenantDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FeatureEntityTypeConfiguration());
    }
}