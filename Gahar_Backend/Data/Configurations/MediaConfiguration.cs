using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    /// <summary>
    /// EF Core configuration for Media entity
    /// </summary>
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
    {
            builder.ToTable("Media");

            // Primary key
builder.HasKey(m => m.Id);

         // Properties
       builder.Property(m => m.FileName)
     .IsRequired()
        .HasMaxLength(255);

 builder.Property(m => m.FilePath)
                .IsRequired()
     .HasMaxLength(500);

         builder.Property(m => m.ThumbnailPath)
         .HasMaxLength(500);

   builder.Property(m => m.WebPPath)
          .HasMaxLength(500);

          builder.Property(m => m.MimeType)
   .IsRequired()
              .HasMaxLength(50);

   builder.Property(m => m.FileSize)
          .IsRequired();

            builder.Property(m => m.WebPFileSize);

   builder.Property(m => m.Width);

  builder.Property(m => m.Height);

         builder.Property(m => m.Alt)
 .HasMaxLength(500);

      builder.Property(m => m.MediaType)
     .IsRequired()
   .HasMaxLength(50);

        builder.Property(m => m.FileExtension)
  .IsRequired()
              .HasMaxLength(10);

       builder.Property(m => m.IsProcessed)
       .IsRequired()
    .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(m => m.MediaType);
    builder.HasIndex(m => m.UploadedBy);
  builder.HasIndex(m => m.CreatedAt);
            builder.HasIndex(m => m.FileName);

       // Relationships
     builder.HasOne(m => m.UploadedByUser)
            .WithMany()
          .HasForeignKey(m => m.UploadedBy)
      .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
