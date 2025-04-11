using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

public class ServicesConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable(nameof(Service));
        builder.HasKey(s => s.Id);

        builder.Property(s => s.ServiceId)
            .IsRequired();

        builder.HasMany(s => s.ServiceTranslations)
            .WithOne(b => b.Services)
            .HasForeignKey(b => b.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Bookings)
            .WithOne(b => b.Service)
            .HasForeignKey(b => b.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(s => s.ETickets)
            .WithOne(e => e.Service)
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(s => s.ServiceGroup)
               .WithMany(g => g.Services)
               .HasForeignKey(s => s.ServiceGroupId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
