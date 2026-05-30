using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Gender).HasMaxLength(20);
        builder.Property(s => s.CurrentInstitution).HasMaxLength(200);
        builder.Property(s => s.CourseOfStudy).HasMaxLength(150);
        builder.Property(s => s.Bio).HasMaxLength(1000);
        builder.Property(s => s.LinkedInUrl).HasMaxLength(300);
        builder.Property(s => s.GitHubUrl).HasMaxLength(300);
        builder.Property(s => s.PortfolioUrl).HasMaxLength(300);
        builder.Property(s => s.Gpa).HasPrecision(4, 2);
        builder.Property(s => s.RowVersion).IsRowVersion();

        builder.HasIndex(s => s.UserId).IsUnique();

        builder.HasMany(s => s.Applications).WithOne(a => a.Student)
            .HasForeignKey(a => a.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(s => s.Educations).WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(s => s.Experiences).WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Resume).WithOne(r => r.Student)
            .HasForeignKey<Resume>(r => r.StudentId).OnDelete(DeleteBehavior.Cascade);
    }
}
