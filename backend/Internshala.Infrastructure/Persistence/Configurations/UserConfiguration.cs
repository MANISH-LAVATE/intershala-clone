using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();
        builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.PhoneNumber).HasMaxLength(15);
        builder.Property(u => u.ProfilePictureUrl).HasMaxLength(500);
        builder.Property(u => u.RowVersion).IsRowVersion();

        builder.HasIndex(u => u.Email).IsUnique().HasFilter("[IsDeleted] = 0");

        builder.HasOne(u => u.Student).WithOne(s => s.User)
            .HasForeignKey<Student>(s => s.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(u => u.Employer).WithOne(e => e.User)
            .HasForeignKey<Employer>(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.RefreshTokens).WithOne(t => t.User)
            .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.Notifications).WithOne(n => n.User)
            .HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
