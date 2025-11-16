using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class PageBlockConfiguration : IEntityTypeConfiguration<PageBlock>
{
    public void Configure(EntityTypeBuilder<PageBlock> builder)
    {
  builder.ToTable("PageBlocks");

builder.HasIndex(pb => new { pb.PageId, pb.DisplayOrder });
    }
}
