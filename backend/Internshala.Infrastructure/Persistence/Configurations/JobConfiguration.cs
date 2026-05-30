using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Title).HasMaxLength(200).IsRequired();
        builder.Property(j => j.Description).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(j => j.Requirements).HasColumnType("nvarchar(max)");
        builder.Property(j => j.JobType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(j => j.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(j => j.RowVersion).IsRowVersion();

        builder.HasOne(j => j.Employer).WithMany(e => e.Jobs)
            .HasForeignKey(j => j.EmployerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(j => j.Category).WithMany()
            .HasForeignKey(j => j.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(j => j.Location).WithMany()
            .HasForeignKey(j => j.LocationId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(j => j.Skills).WithMany(s => s.Jobs)
            .UsingEntity("JobSkills");

        builder.HasIndex(j => new { j.IsDeleted, j.IsActive, j.Status, j.CategoryId });
    }
}
