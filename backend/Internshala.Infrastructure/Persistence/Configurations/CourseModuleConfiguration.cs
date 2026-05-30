using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModule>
{
    public void Configure(EntityTypeBuilder<CourseModule> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title).HasMaxLength(300).IsRequired();
        builder.Property(m => m.Description).HasColumnType("nvarchar(max)");
        builder.Property(m => m.VideoUrl).HasMaxLength(1000);
        builder.Property(m => m.Content).HasColumnType("nvarchar(max)");
        builder.Property(m => m.RowVersion).IsRowVersion();

        builder.HasIndex(m => new { m.CourseId, m.OrderIndex }).IsUnique();
    }
}
