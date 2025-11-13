using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
 {
      builder.ToTable("Facilities");

        builder.HasIndex(f => f.Slug).IsUnique();

  builder.HasOne(f => f.Author)
       .WithMany()
         .HasForeignKey(f => f.AuthorId)
         .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(f => f.Departments)
 .WithOne(fd => fd.Facility)
      .HasForeignKey(fd => fd.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.Services)
         .WithOne(fs => fs.Facility)
    .HasForeignKey(fs => fs.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.Images)
     .WithOne(fi => fi.Facility)
   .HasForeignKey(fi => fi.FacilityId)
   .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.Reviews)
          .WithOne(fr => fr.Facility)
   .HasForeignKey(fr => fr.FacilityId)
       .OnDelete(DeleteBehavior.Cascade);
    }
}
