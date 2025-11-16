using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class FacilityServiceConfiguration : IEntityTypeConfiguration<FacilityService>
{
public void Configure(EntityTypeBuilder<FacilityService> builder)
    {
      builder.ToTable("FacilityServices");

     builder.HasIndex(fs => new { fs.FacilityId, fs.DisplayOrder });
    }
}
