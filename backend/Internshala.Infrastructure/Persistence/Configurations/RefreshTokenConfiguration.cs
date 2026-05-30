using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.TokenHash).HasMaxLength(64).IsRequired();
        builder.Property(r => r.ReplacedByToken).HasMaxLength(64);
        builder.Property(r => r.UserAgent).HasMaxLength(300);
        builder.Property(r => r.IpAddress).HasMaxLength(45);

        builder.HasIndex(r => r.TokenHash);
        builder.HasIndex(r => new { r.UserId, r.RevokedAt });

        builder.Ignore(r => r.IsRevoked);
        builder.Ignore(r => r.IsExpired);
        builder.Ignore(r => r.IsActive);
    }
}
