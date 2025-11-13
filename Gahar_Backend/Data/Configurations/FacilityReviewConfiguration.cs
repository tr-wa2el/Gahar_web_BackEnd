using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class FacilityReviewConfiguration : IEntityTypeConfiguration<FacilityReview>
{
    public void Configure(EntityTypeBuilder<FacilityReview> builder)
   {
  builder.ToTable("FacilityReviews");

 builder.HasIndex(fr => new { fr.FacilityId, fr.IsApproved, fr.CreatedAt }).IsDescending(false, false, true);
  }
}
