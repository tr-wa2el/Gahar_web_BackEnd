using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
  builder.ToTable("Contents");

builder.HasIndex(c => new { c.ContentTypeId, c.Slug }).IsUnique();
        builder.HasIndex(c => new { c.Status, c.PublishedAt }).IsDescending(false, true);
    builder.HasIndex(c => c.AuthorId);
        builder.HasIndex(c => new { c.IsFeatured, c.PublishedAt }).IsDescending(false, true);
        builder.HasIndex(c => c.ViewCount).IsDescending(true);

    builder.HasMany(c => c.FieldValues)
            .WithOne(fv => fv.Content)
            .HasForeignKey(fv => fv.ContentId)
 .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Tags)
 .WithOne(ct => ct.Content)
       .HasForeignKey(ct => ct.ContentId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ContentFieldValueConfiguration : IEntityTypeConfiguration<ContentFieldValue>
{
    public void Configure(EntityTypeBuilder<ContentFieldValue> builder)
    {
    builder.ToTable("ContentFieldValues");

        builder.HasIndex(cfv => new { cfv.ContentId, cfv.FieldKey, cfv.LanguageId }).IsUnique();
    }
}

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");

     builder.HasIndex(t => t.Slug).IsUnique();

  builder.HasMany(t => t.ContentTags)
     .WithOne(ct => ct.Tag)
            .HasForeignKey(ct => ct.TagId)
   .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ContentTagConfiguration : IEntityTypeConfiguration<ContentTag>
{
    public void Configure(EntityTypeBuilder<ContentTag> builder)
    {
  builder.ToTable("ContentTags");

        builder.HasIndex(ct => new { ct.ContentId, ct.TagId }).IsUnique();
 }
}

public class LayoutConfiguration : IEntityTypeConfiguration<Layout>
{
    public void Configure(EntityTypeBuilder<Layout> builder)
  {
 builder.ToTable("Layouts");

    builder.HasIndex(l => l.IsDefault);

      builder.HasMany(l => l.Contents)
    .WithOne(c => c.Layout)
            .HasForeignKey(c => c.LayoutId)
     .OnDelete(DeleteBehavior.SetNull);
    }
}
