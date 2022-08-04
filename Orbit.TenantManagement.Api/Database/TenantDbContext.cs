using Microsoft.EntityFrameworkCore;
using Orbit.TenantManagement.Api.Database.Mappings;
using Orbit.TenantManagement.Api.Models;

namespace Orbit.TenantManagement.Api.Database;

public class TenantDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<TenantFeature> TenantFeatures { get; set; }

    public TenantDbContext(DbContextOptions<TenantDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FeatureEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TenantFeatureEntityTypeConfiguration());
    }
}