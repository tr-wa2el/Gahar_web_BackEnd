using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
 {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
{
   builder.ToTable("AuditLogs");

            builder.HasKey(al => al.Id);

            builder.HasIndex(al => al.UserId);
            builder.HasIndex(al => al.EntityType);
 builder.HasIndex(al => al.Timestamp);
            builder.HasIndex(al => new { al.EntityType, al.EntityId });

            builder.Property(al => al.Action)
       .IsRequired()
      .HasMaxLength(50);

 builder.Property(al => al.EntityType)
.IsRequired()
  .HasMaxLength(100);

         builder.Property(al => al.Description)
 .HasMaxLength(500);

    builder.Property(al => al.IpAddress)
         .HasMaxLength(45);

   builder.Property(al => al.UserAgent)
  .HasMaxLength(500);

   builder.Property(al => al.Timestamp)
    .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(al => al.CreatedAt)
      .HasDefaultValueSql("GETUTCDATE()");

  // Relationships
       builder.HasOne(al => al.User)
    .WithMany(u => u.AuditLogs)
      .HasForeignKey(al => al.UserId)
   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
