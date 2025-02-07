using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable(nameof(Booking));
        builder.HasKey(c => c.Id);

        builder.Property(b => b.ServiceId)
            .IsRequired();

        builder.Property(b => b.StartDate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.StartTime)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(b => b.Language)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(b => b.BookingCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Success)
            .IsRequired();

        builder.Property(b => b.ServiceName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.BranchName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Service)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
