using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
 {
        public void Configure(EntityTypeBuilder<Permission> builder)
 {
   builder.ToTable("Permissions");

builder.HasKey(p => p.Id);

          builder.HasIndex(p => new { p.Name, p.Module })
       .IsUnique();

            builder.Property(p => p.Name)
    .IsRequired()
    .HasMaxLength(100);

  builder.Property(p => p.Module)
    .IsRequired()
       .HasMaxLength(50);

       builder.Property(p => p.Description)
.HasMaxLength(500);

    builder.Property(p => p.CreatedAt)
.HasDefaultValueSql("GETUTCDATE()");

   // Relationships
       builder.HasMany(p => p.RolePermissions)
   .WithOne(rp => rp.Permission)
    .HasForeignKey(rp => rp.PermissionId)
     .OnDelete(DeleteBehavior.Cascade);
 }
    }
}
