using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Institution).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Degree).HasMaxLength(150).IsRequired();
        builder.Property(e => e.FieldOfStudy).HasMaxLength(150).IsRequired();
        builder.Property(e => e.Grade).HasPrecision(4, 2);
        builder.Property(e => e.RowVersion).IsRowVersion();
    }
}
