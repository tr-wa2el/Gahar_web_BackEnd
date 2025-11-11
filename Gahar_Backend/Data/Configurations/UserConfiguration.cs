using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
    public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

    builder.HasKey(u => u.Id);

    builder.HasIndex(u => u.Email)
     .IsUnique();

            builder.HasIndex(u => u.Username)
   .IsUnique();

            builder.Property(u => u.Username)
     .IsRequired()
    .HasMaxLength(100);

   builder.Property(u => u.Email)
         .IsRequired()
 .HasMaxLength(200);

       builder.Property(u => u.PasswordHash)
 .IsRequired();

            builder.Property(u => u.FirstName)
     .HasMaxLength(100);

          builder.Property(u => u.LastName)
    .HasMaxLength(100);

            builder.Property(u => u.PhoneNumber)
              .HasMaxLength(20);

          builder.Property(u => u.IsActive)
     .HasDefaultValue(true);

            builder.Property(u => u.EmailConfirmed)
       .HasDefaultValue(false);

            builder.Property(u => u.FailedLoginAttempts)
                .HasDefaultValue(0);

            builder.Property(u => u.CreatedAt)
.HasDefaultValueSql("GETUTCDATE()");

   // Relationships
     builder.HasMany(u => u.UserRoles)
     .WithOne(ur => ur.User)
           .HasForeignKey(ur => ur.UserId)
      .OnDelete(DeleteBehavior.Cascade);

       builder.HasMany(u => u.AuditLogs)
     .WithOne(al => al.User)
                .HasForeignKey(al => al.UserId)
      .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
