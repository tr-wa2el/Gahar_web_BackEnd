using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
      public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(ur => ur.Id);

     builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
      .IsUnique();

       builder.Property(ur => ur.AssignedAt)
         .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(ur => ur.CreatedAt)
      .HasDefaultValueSql("GETUTCDATE()");

            // Relationships configured in User and Role configurations
 }
    }
}
