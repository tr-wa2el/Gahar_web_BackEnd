using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
  public void Configure(EntityTypeBuilder<RolePermission> builder)
{
   builder.ToTable("RolePermissions");

    builder.HasKey(rp => rp.Id);

          builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
   .IsUnique();

      builder.Property(rp => rp.GrantedAt)
    .HasDefaultValueSql("GETUTCDATE()");

       builder.Property(rp => rp.CreatedAt)
       .HasDefaultValueSql("GETUTCDATE()");

   // Relationships configured in Role and Permission configurations
  }
    }
}
