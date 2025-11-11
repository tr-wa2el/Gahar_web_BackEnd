using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
{
    builder.ToTable("Roles");

    builder.HasKey(r => r.Id);

   builder.HasIndex(r => r.Name)
       .IsUnique();

     builder.Property(r => r.Name)
    .IsRequired()
.HasMaxLength(50);

        builder.Property(r => r.DisplayName)
.IsRequired()
  .HasMaxLength(100);

   builder.Property(r => r.Description)
     .HasMaxLength(500);

 builder.Property(r => r.IsSystemRole)
.HasDefaultValue(false);

            builder.Property(r => r.CreatedAt)
     .HasDefaultValueSql("GETUTCDATE()");

     // Relationships
            builder.HasMany(r => r.UserRoles)
       .WithOne(ur => ur.Role)
 .HasForeignKey(ur => ur.RoleId)
    .OnDelete(DeleteBehavior.Cascade);

    builder.HasMany(r => r.RolePermissions)
    .WithOne(rp => rp.Role)
    .HasForeignKey(rp => rp.RoleId)
 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
