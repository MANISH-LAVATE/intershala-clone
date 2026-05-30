using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class ResumeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.FileUrl).HasMaxLength(500);
        builder.Property(r => r.FileName).HasMaxLength(200);
        builder.Property(r => r.RowVersion).IsRowVersion();
        builder.HasIndex(r => r.StudentId).IsUnique();
    }
}
