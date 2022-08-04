using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Tenant.Api.Models;

namespace Orbit.Tenant.Api.Database.Mappings;

public class FeatureEntityTypeConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.ToTable("feature");

        builder.Property(e => e.FeatureId).HasColumnName("feature_id");

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
    }
}