using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    public void Configure(EntityTypeBuilder<Page> builder)
    {
        builder.ToTable("Pages");

        builder.HasIndex(p => p.Slug).IsUnique();

    builder.HasOne(p => p.Author)
     .WithMany()
            .HasForeignKey(p => p.AuthorId)
    .OnDelete(DeleteBehavior.SetNull);

   builder.HasMany(p => p.Blocks)
            .WithOne(b => b.Page)
.HasForeignKey(b => b.PageId)
   .OnDelete(DeleteBehavior.Cascade);
    }
}
