using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class FacilityDepartmentConfiguration : IEntityTypeConfiguration<FacilityDepartment>
{
    public void Configure(EntityTypeBuilder<FacilityDepartment> builder)
 {
    builder.ToTable("FacilityDepartments");

   builder.HasIndex(fd => new { fd.FacilityId, fd.DisplayOrder });
    }
}
