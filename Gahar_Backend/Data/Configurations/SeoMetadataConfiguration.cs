using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class SeoMetadataConfiguration : IEntityTypeConfiguration<SeoMetadata>
{
    public void Configure(EntityTypeBuilder<SeoMetadata> builder)
    {
        builder.ToTable("SeoMetadata");

 builder.HasIndex(s => new { s.EntityType, s.EntityId }).IsUnique();
        builder.HasIndex(s => s.CreatedAt).IsDescending(true);
    }
}
