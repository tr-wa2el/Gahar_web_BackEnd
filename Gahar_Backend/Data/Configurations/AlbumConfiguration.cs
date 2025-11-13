using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Albums");

   builder.HasIndex(a => a.Slug).IsUnique();
        builder.HasIndex(a => a.IsPublished);
   builder.HasIndex(a => a.CreatedBy);
  builder.HasIndex(a => a.PublishedAt).IsDescending(true);
 builder.HasIndex(a => a.ViewCount).IsDescending(true);

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

public class AlbumMediaConfiguration : IEntityTypeConfiguration<AlbumMedia>
{
   public void Configure(EntityTypeBuilder<AlbumMedia> builder)
    {
        builder.ToTable("AlbumMedias");

  builder.HasIndex(am => new { am.AlbumId, am.DisplayOrder });
      builder.HasIndex(am => new { am.AlbumId, am.IsFeatured });

     builder.HasOne(am => am.Media)
      .WithMany()
        .HasForeignKey(am => am.MediaId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
