using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal class CompaniesConfiguration : IEntityTypeConfiguration<Companies>
{
    public void Configure(EntityTypeBuilder<Companies> builder)
    {
        builder.ToTable(nameof(Companies));
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description);

        builder.Property(c => c.BaseUrlForOnlinet)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.BaseUrlForBookingService)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.LogoBase64);

        builder.Property(c => c.Color)
            .HasMaxLength(50);

        builder.Property(c => c.BackgroundColor)
            .HasMaxLength(50);

        builder.HasMany(c => c.Branches)
            .WithOne(s => s.Companies)
            .HasForeignKey(s => s.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
