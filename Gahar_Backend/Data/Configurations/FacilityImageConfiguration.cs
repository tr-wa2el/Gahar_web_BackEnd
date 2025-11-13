using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class FacilityImageConfiguration : IEntityTypeConfiguration<FacilityImage>
{
   public void Configure(EntityTypeBuilder<FacilityImage> builder)
    {
   builder.ToTable("FacilityImages");

    builder.HasIndex(fi => new { fi.FacilityId, fi.DisplayOrder });
   }
}
