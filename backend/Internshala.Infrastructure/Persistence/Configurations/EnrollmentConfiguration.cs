using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.HasOne(e => e.Student).WithMany()
            .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Course).WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate enrollments
        builder.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
