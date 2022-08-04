using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Database.Mappings;
using Orbit.Multitenant.Api.Middleware;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Database;

public class OrbitDbContext : DbContext
{
    private readonly IDomainContextInfo _domainContextInfo;
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Feature> Features { get; set; }

    public OrbitDbContext(DbContextOptions<OrbitDbContext> options, IDomainContextInfo domainContextInfo) 
        : base(options)
    {
        _domainContextInfo = domainContextInfo;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FeatureEntityTypeConfiguration());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _domainContextInfo.TenantId = new Guid("c7c6a582-4031-4e48-b8bf-f1a5289c9997");
        if (_domainContextInfo.TenantId.HasValue)
        {
            var connection = GetConnection(_domainContextInfo.TenantId.Value);
            optionsBuilder.UseNpgsql(connection);
        }
    }

    private string GetConnection(Guid tenantId)
    {
        return $"Server=localhost;Port=5432;Database=orbit-test;User Id={tenantId};Password=123;";
    }
}