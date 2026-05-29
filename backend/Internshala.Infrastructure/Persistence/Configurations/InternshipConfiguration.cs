using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class InternshipConfiguration : IEntityTypeConfiguration<Internship>
{
    public void Configure(EntityTypeBuilder<Internship> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Title).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Description).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(i => i.Responsibilities).HasColumnType("nvarchar(max)");
        builder.Property(i => i.Requirements).HasColumnType("nvarchar(max)");
        builder.Property(i => i.InternshipType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(i => i.StartDateType).HasMaxLength(20).IsRequired();
        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(i => i.RowVersion).IsRowVersion();

        builder.HasOne(i => i.Employer).WithMany(e => e.Internships)
            .HasForeignKey(i => i.EmployerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(i => i.Category).WithMany()
            .HasForeignKey(i => i.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(i => i.Location).WithMany()
            .HasForeignKey(i => i.LocationId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(i => i.Applications).WithOne(a => a.Internship)
            .HasForeignKey(a => a.InternshipId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(i => i.Skills).WithMany(s => s.Internships)
            .UsingEntity("InternshipSkills");

        builder.HasIndex(i => new { i.IsDeleted, i.IsActive, i.Status, i.CategoryId, i.LocationId });
    }
}
