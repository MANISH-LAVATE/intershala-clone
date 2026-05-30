using Internshala.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internshala.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.CityName).HasMaxLength(100).IsRequired();
        builder.Property(l => l.State).HasMaxLength(100).IsRequired();
        builder.HasIndex(l => l.CityName);
    }
}
