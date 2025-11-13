using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
  builder.ToTable("Menus");

 builder.HasIndex(m => m.Slug).IsUnique();

        builder.HasOne(m => m.Author)
     .WithMany()
            .HasForeignKey(m => m.AuthorId)
    .OnDelete(DeleteBehavior.SetNull);

    builder.HasMany(m => m.Items)
            .WithOne(mi => mi.Menu)
            .HasForeignKey(mi => mi.MenuId)
      .OnDelete(DeleteBehavior.Cascade);
    }
}
