using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> builder)
    {
  builder.ToTable("Keywords");

        builder.HasIndex(k => k.Term).IsUnique();
        builder.HasIndex(k => new { k.TargetEntity, k.TargetEntityId });
        builder.HasIndex(k => k.Difficulty);
  builder.HasIndex(k => k.SearchVolume).IsDescending(true);
    }
}
