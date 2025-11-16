using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class FormConfiguration : IEntityTypeConfiguration<Form>
{
    public void Configure(EntityTypeBuilder<Form> builder)
    {
      builder.ToTable("Forms");

       builder.HasIndex(f => f.Slug).IsUnique();

        builder.HasOne(f => f.Author)
     .WithMany()
    .HasForeignKey(f => f.AuthorId)
     .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(f => f.Fields)
.WithOne(ff => ff.Form)
            .HasForeignKey(ff => ff.FormId)
      .OnDelete(DeleteBehavior.Cascade);

      builder.HasMany(f => f.Submissions)
 .WithOne(fs => fs.Form)
         .HasForeignKey(fs => fs.FormId)
 .OnDelete(DeleteBehavior.Cascade);
    }
}
