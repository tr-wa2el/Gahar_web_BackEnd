using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    /// <summary>
    /// EF Core configuration for Album entity
    /// </summary>
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("Albums");

  // Primary key
          builder.HasKey(a => a.Id);

         // Properties
        builder.Property(a => a.Title)
         .IsRequired()
     .HasMaxLength(200);

       builder.Property(a => a.Slug)
           .IsRequired()
     .HasMaxLength(200);

     builder.Property(a => a.Description)
.HasMaxLength(1000);

     builder.Property(a => a.ViewCount)
                .IsRequired()
                .HasDefaultValue(0);

   builder.Property(a => a.IsPublished)
  .IsRequired()
              .HasDefaultValue(false);

    // Indexes
  builder.HasIndex(a => a.Slug).IsUnique();
     builder.HasIndex(a => a.IsPublished);
          builder.HasIndex(a => a.CreatedBy);
            builder.HasIndex(a => a.CreatedAt);

     // Relationships
   builder.HasOne(a => a.CoverImage)
  .WithMany()
   .HasForeignKey(a => a.CoverImageId)
                .OnDelete(DeleteBehavior.SetNull);

       builder.HasOne(a => a.Creator)
            .WithMany()
        .HasForeignKey(a => a.CreatedBy)
     .OnDelete(DeleteBehavior.SetNull);

          builder.HasMany(a => a.AlbumMedias)
     .WithOne(am => am.Album)
           .HasForeignKey(am => am.AlbumId)
      .OnDelete(DeleteBehavior.Cascade);
        }
    }

    /// <summary>
    /// EF Core configuration for AlbumMedia entity
    /// </summary>
    public class AlbumMediaConfiguration : IEntityTypeConfiguration<AlbumMedia>
    {
      public void Configure(EntityTypeBuilder<AlbumMedia> builder)
        {
       builder.ToTable("AlbumMedias");

    // Primary key
            builder.HasKey(am => am.Id);

          // Properties
            builder.Property(am => am.DisplayOrder)
          .IsRequired();

  builder.Property(am => am.Caption)
                .HasMaxLength(500);

        builder.Property(am => am.IsFeatured)
                .IsRequired()
 .HasDefaultValue(false);

            // Unique constraint on Album + Media combination
builder.HasIndex(am => new { am.AlbumId, am.MediaId })
     .IsUnique();

  // Index on display order for sorting
  builder.HasIndex(am => am.DisplayOrder);

            // Relationships
         builder.HasOne(am => am.Album)
      .WithMany(a => a.AlbumMedias)
        .HasForeignKey(am => am.AlbumId)
          .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(am => am.Media)
          .WithMany()
            .HasForeignKey(am => am.MediaId)
          .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
