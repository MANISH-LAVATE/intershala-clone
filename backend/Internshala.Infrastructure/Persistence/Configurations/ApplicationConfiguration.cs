using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainApplication = Internshala.Domain.Entities.Application;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<DomainApplication>
{
    public void Configure(EntityTypeBuilder<DomainApplication> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.ListingType).HasMaxLength(10).IsRequired();
        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(a => a.CoverLetter).HasMaxLength(2000);
        builder.Property(a => a.ResumeUrl).HasMaxLength(500).IsRequired();
        builder.Property(a => a.EmployerNote).HasMaxLength(1000);
        builder.Property(a => a.RejectionReason).HasMaxLength(500);
        builder.Property(a => a.RowVersion).IsRowVersion();

        builder.HasOne(a => a.Student).WithMany(s => s.Applications)
            .HasForeignKey(a => a.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Internship).WithMany(i => i.Applications)
            .HasForeignKey(a => a.InternshipId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
        builder.HasOne(a => a.Job).WithMany(j => j.Applications)
            .HasForeignKey(a => a.JobId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
        builder.HasMany(a => a.StatusHistory).WithOne(h => h.Application)
            .HasForeignKey(h => h.ApplicationId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new { a.StudentId, a.InternshipId })
            .IsUnique()
            .HasFilter("[InternshipId] IS NOT NULL AND [IsDeleted] = 0");

        builder.HasIndex(a => new { a.StudentId, a.IsDeleted });
        builder.HasIndex(a => new { a.InternshipId, a.IsDeleted });
    }
}
