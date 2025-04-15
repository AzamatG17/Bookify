using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal class ServiceGroupTranslationConfiguration : IEntityTypeConfiguration<ServiceGroupTranslation>
{
    public void Configure(EntityTypeBuilder<ServiceGroupTranslation> builder)
    {
        builder.ToTable(nameof(ServiceGroupTranslation));
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(s => s.LanguageCode)
            .HasMaxLength(10)
            .IsRequired();
    }
}
