using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Multitenant.Api.Database.Models;

namespace Orbit.Multitenant.Api.Database.Mappings;

public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder
            .ToTable("tenant", "public")
            .HasKey(x => x.TenantId);

        builder
            .Property(x => x.TenantId)
            .HasColumnName("tenant_id");

        builder
            .Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();
    }
}