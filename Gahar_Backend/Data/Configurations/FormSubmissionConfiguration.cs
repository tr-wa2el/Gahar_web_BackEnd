using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class FormSubmissionConfiguration : IEntityTypeConfiguration<FormSubmission>
{
public void Configure(EntityTypeBuilder<FormSubmission> builder)
    {
        builder.ToTable("FormSubmissions");

      builder.HasIndex(fs => new { fs.FormId, fs.CreatedAt }).IsDescending(false, true);
        builder.HasIndex(fs => fs.SubmitterEmail);
  builder.HasIndex(fs => fs.IsRead);
    }
}
