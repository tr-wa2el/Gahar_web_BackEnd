using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentTagConfiguration : IEntityTypeConfiguration<ContentTag>
    {
  public void Configure(EntityTypeBuilder<ContentTag> builder)
  {
    builder.ToTable("ContentTags");

       builder.HasKey(ct => ct.Id);

     // Composite unique index
    builder.HasIndex(ct => new { ct.ContentId, ct.TagId })
  .IsUnique();

     builder.Property(ct => ct.CreatedAt)
.HasDefaultValueSql("GETUTCDATE()");

            // Relationships configured in Content and Tag configurations
     }
    }
}
