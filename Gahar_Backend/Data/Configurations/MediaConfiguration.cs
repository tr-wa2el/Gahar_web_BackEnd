using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
builder.ToTable("Media");

 builder.HasIndex(m => m.MimeType);
        builder.HasIndex(m => m.MediaType);
        builder.HasIndex(m => m.UploadedBy);
        builder.HasIndex(m => m.IsProcessed);
     builder.HasIndex(m => m.CreatedAt).IsDescending(true);
 builder.HasIndex(m => m.UsageCount).IsDescending(true);

        builder.HasOne(m => m.Uploader)
            .WithMany()
     .HasForeignKey(m => m.UploadedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
