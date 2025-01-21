using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

public class ServicesConfiguration : IEntityTypeConfiguration<Services>
{
    public void Configure(EntityTypeBuilder<Services> builder)
    {
        builder.ToTable(nameof(Services));
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Location)
            .HasMaxLength(100)
            .IsRequired();
    }
}
