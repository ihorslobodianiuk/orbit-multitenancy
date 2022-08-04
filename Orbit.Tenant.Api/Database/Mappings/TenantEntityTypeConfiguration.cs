using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orbit.Tenant.Api.Database.Mappings;

public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Models.Tenant>
{
    public void Configure(EntityTypeBuilder<Models.Tenant> builder)
    {
        builder.ToTable("tenant");

        builder.HasIndex(e => e.Name, "tenant_name_key")
            .IsUnique();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");

        builder.Property(e => e.Status)
            .HasMaxLength(64)
            .HasColumnName("status");

        builder.Property(e => e.Tier)
            .HasMaxLength(64)
            .HasColumnName("tier");
    }
}