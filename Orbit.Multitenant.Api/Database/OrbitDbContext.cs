using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Orbit.Multitenant.Api.Database.Mappings;
using Orbit.Multitenant.Api.Database.Models;
using Orbit.Multitenant.Api.Middleware;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Database;

public class OrbitDbContext : DbContext
{
    private readonly IDomainContextInfo _domainContextInfo;
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    public OrbitDbContext(DbContextOptions<OrbitDbContext> options, IDomainContextInfo domainContextInfo) 
        : base(options)
    {
        _domainContextInfo = domainContextInfo;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _domainContextInfo.TenantName = "c7c6a582-4031-4e48-b8bf-f1a5289c9997";
        if (!string.IsNullOrEmpty(_domainContextInfo.TenantName))
        {
            var connection = GetConnection(_domainContextInfo.TenantName);
            optionsBuilder.UseNpgsql(connection);
        }
    }

    private string GetConnection(string tenantName)
    {
        return $"Server=localhost;Port=5432;Database=orbit-test;User Id={tenantName};Password=123;";
    }
}