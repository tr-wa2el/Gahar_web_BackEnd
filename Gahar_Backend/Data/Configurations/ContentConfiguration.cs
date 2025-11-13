using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
      {
      builder.ToTable("Contents");

       builder.HasKey(c => c.Id);

     // Composite unique index on ContentTypeId and Slug
      builder.HasIndex(c => new { c.ContentTypeId, c.Slug })
    .IsUnique();

     // Index on Status for filtering
      builder.HasIndex(c => c.Status);

        // Index on PublishedAt for sorting
   builder.HasIndex(c => c.PublishedAt);

   // Index on IsFeatured for filtering
     builder.HasIndex(c => c.IsFeatured);

     // Properties
       builder.Property(c => c.Title)
       .IsRequired()
      .HasMaxLength(200);

         builder.Property(c => c.Slug)
       .IsRequired()
    .HasMaxLength(200);

   builder.Property(c => c.FeaturedImage)
       .HasMaxLength(500);

            builder.Property(c => c.Status)
                .IsRequired()
    .HasMaxLength(50)
             .HasDefaultValue(ContentStatus.Draft);

   builder.Property(c => c.ViewCount)
       .HasDefaultValue(0);

   builder.Property(c => c.IsFeatured)
  .HasDefaultValue(false);

            builder.Property(c => c.AllowComments)
          .HasDefaultValue(true);

     // SEO Properties
    builder.Property(c => c.MetaTitle)
       .HasMaxLength(200);

    builder.Property(c => c.MetaDescription)
  .HasMaxLength(500);

            builder.Property(c => c.OgTitle)
        .HasMaxLength(200);

        builder.Property(c => c.OgDescription)
           .HasMaxLength(500);

 builder.Property(c => c.OgImage)
    .HasMaxLength(500);

       builder.Property(c => c.CreatedAt)
         .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
  builder.HasOne(c => c.ContentType)
   .WithMany(ct => ct.Contents)
    .HasForeignKey(c => c.ContentTypeId)
 .OnDelete(DeleteBehavior.Restrict);

       builder.HasOne(c => c.Author)
 .WithMany()
           .HasForeignKey(c => c.AuthorId)
       .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.FieldValues)
     .WithOne(fv => fv.Content)
      .HasForeignKey(fv => fv.ContentId)
     .OnDelete(DeleteBehavior.Cascade);

      builder.HasMany(c => c.ContentTags)
   .WithOne(ct => ct.Content)
     .HasForeignKey(ct => ct.ContentId)
        .OnDelete(DeleteBehavior.Cascade);
  }
    }
}
