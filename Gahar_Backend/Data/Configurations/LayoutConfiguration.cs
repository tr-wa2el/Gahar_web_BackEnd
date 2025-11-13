using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    /// <summary>
    /// EF Core configuration for Layout entity
    /// </summary>
    public class LayoutConfiguration : IEntityTypeConfiguration<Layout>
    {
        public void Configure(EntityTypeBuilder<Layout> builder)
        {
     builder.ToTable("Layouts");

     // Primary key
   builder.HasKey(l => l.Id);

   // Properties
      builder.Property(l => l.Name)
           .IsRequired()
   .HasMaxLength(100);

            builder.Property(l => l.Description)
      .HasMaxLength(500);

            builder.Property(l => l.Configuration)
        .IsRequired()
          .HasColumnType("nvarchar(max)");

    builder.Property(l => l.IsDefault)
      .IsRequired()
         .HasDefaultValue(false);

    builder.Property(l => l.IsActive)
           .IsRequired()
      .HasDefaultValue(true);

        builder.Property(l => l.PreviewImage)
     .HasMaxLength(500);

       // Indexes
            builder.HasIndex(l => l.Name)
          .IsUnique();

  builder.HasIndex(l => l.IsDefault);

      builder.HasIndex(l => l.IsActive);

            // Relationships
   builder.HasMany(l => l.Contents)
   .WithOne(c => c.Layout)
             .HasForeignKey(c => c.LayoutId)
      .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
