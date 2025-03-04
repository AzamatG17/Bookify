using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

public class PredefinedTextConfiguration : IEntityTypeConfiguration<PredefinedText>
{
    public void Configure(EntityTypeBuilder<PredefinedText> builder)
    {
        builder.ToTable(nameof(PredefinedText));
        builder.HasKey(c => c.Id);

        builder.Property(pt => pt.Text)
            .IsRequired()
            .HasMaxLength(500);
    }
}
