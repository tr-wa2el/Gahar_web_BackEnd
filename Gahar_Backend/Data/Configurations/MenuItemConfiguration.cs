using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
     builder.ToTable("MenuItems");

        builder.HasIndex(mi => new { mi.MenuId, mi.DisplayOrder });
        
        builder.HasOne(mi => mi.Menu)
   .WithMany(m => m.Items)
       .HasForeignKey(mi => mi.MenuId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mi => mi.ParentItem)
 .WithMany(mi => mi.ChildItems)
  .HasForeignKey(mi => mi.ParentItemId)
          .OnDelete(DeleteBehavior.NoAction);
    }
}
