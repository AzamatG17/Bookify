using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

public class ServiceGroupConfiguration : IEntityTypeConfiguration<ServiceGroup>
{
    public void Configure(EntityTypeBuilder<ServiceGroup> builder)
    {
        builder.ToTable(nameof(ServiceGroup));
        builder.HasKey(s => s.Id);

        builder.Property(g => g.Name)
               .IsRequired()
               .HasMaxLength(2000);
    }
}
