using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentTypeConfiguration : IEntityTypeConfiguration<ContentType>
    {
      public void Configure(EntityTypeBuilder<ContentType> builder)
      {
    builder.ToTable("ContentTypes");

            builder.HasKey(ct => ct.Id);

            // Unique constraint on Slug
    builder.HasIndex(ct => ct.Slug)
        .IsUnique();

     // Properties
    builder.Property(ct => ct.Name)
   .IsRequired()
             .HasMaxLength(100);

            builder.Property(ct => ct.Slug)
   .IsRequired()
          .HasMaxLength(100);

         builder.Property(ct => ct.Description)
      .HasMaxLength(500);

            builder.Property(ct => ct.Icon)
                .HasMaxLength(50)
         .HasDefaultValue("FileText");

            builder.Property(ct => ct.IsSinglePage)
  .HasDefaultValue(false);

   builder.Property(ct => ct.IsActive)
       .HasDefaultValue(true);

  builder.Property(ct => ct.MetaTitleTemplate)
         .HasMaxLength(200);

   builder.Property(ct => ct.MetaDescriptionTemplate)
          .HasMaxLength(500);

        builder.Property(ct => ct.CreatedAt)
         .HasDefaultValueSql("GETUTCDATE()");

       // Relationships
        builder.HasMany(ct => ct.Fields)
       .WithOne(f => f.ContentType)
  .HasForeignKey(f => f.ContentTypeId)
        .OnDelete(DeleteBehavior.Cascade);

   builder.HasMany(ct => ct.Contents)
       .WithOne(c => c.ContentType)
           .HasForeignKey(c => c.ContentTypeId)
   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
