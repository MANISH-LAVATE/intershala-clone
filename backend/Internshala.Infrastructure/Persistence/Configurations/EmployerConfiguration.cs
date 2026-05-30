using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CompanyName).HasMaxLength(200).IsRequired();
        builder.Property(e => e.CompanySize).HasMaxLength(30);
        builder.Property(e => e.CompanyWebsite).HasMaxLength(300);
        builder.Property(e => e.CompanyLogoUrl).HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.HeadquartersCity).HasMaxLength(100);
        builder.Property(e => e.LinkedInUrl).HasMaxLength(300);
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.HasIndex(e => e.UserId).IsUnique();

        builder.HasMany(e => e.Internships).WithOne(i => i.Employer)
            .HasForeignKey(i => i.EmployerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Jobs).WithOne(j => j.Employer)
            .HasForeignKey(j => j.EmployerId).OnDelete(DeleteBehavior.Restrict);
    }
}
