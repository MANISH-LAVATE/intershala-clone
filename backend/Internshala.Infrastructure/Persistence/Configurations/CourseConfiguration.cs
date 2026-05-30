using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).HasMaxLength(300).IsRequired();
        builder.Property(c => c.Description).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(c => c.Instructor).HasMaxLength(200);
        builder.Property(c => c.ThumbnailUrl).HasMaxLength(1000);
        builder.Property(c => c.Level).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(c => c.Language).HasMaxLength(50);
        builder.Property(c => c.Prerequisites).HasColumnType("nvarchar(max)");
        builder.Property(c => c.WhatYouLearn).HasColumnType("nvarchar(max)");
        builder.Property(c => c.Price).HasPrecision(10, 2);
        builder.Property(c => c.RowVersion).IsRowVersion();

        builder.HasOne(c => c.Category).WithMany()
            .HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(c => c.Modules).WithOne(m => m.Course)
            .HasForeignKey(m => m.CourseId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Enrollments).WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => new { c.IsDeleted, c.IsPublished, c.CategoryId });
    }
}
