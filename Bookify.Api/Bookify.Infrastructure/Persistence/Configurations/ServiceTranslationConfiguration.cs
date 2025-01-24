using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal class ServiceTranslationConfiguration : IEntityTypeConfiguration<ServiceTranslation>
{
    public void Configure(EntityTypeBuilder<ServiceTranslation> builder)
    {
        builder.ToTable(nameof(ServiceTranslation));
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(s => s.LanguageCode)
            .HasMaxLength(10)
            .IsRequired();

        //builder.HasOne(s => s.Services)
        //    .WithMany(st => st.ServiceTranslations)
        //    .HasForeignKey(s => s.ServiceId)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}
