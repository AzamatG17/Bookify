using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

public class ServiceRatingConfiguration : IEntityTypeConfiguration<ServiceRating>
{
    public void Configure(EntityTypeBuilder<ServiceRating> builder)
    {
        builder.ToTable(nameof(ServiceRating));
        builder.HasKey(c => c.Id);

        builder.Property(sr => sr.Comment)
            .HasMaxLength(3000);

        builder.Property(sr => sr.TicketNumber)
            .HasMaxLength(100);

        builder.Property(sr => sr.DeskNumber)
            .HasMaxLength(50);

        builder.Property(sr => sr.DeskName)
            .HasMaxLength(100);

        builder.Property(sr => sr.SmileyRating)
            .IsRequired();

        builder.HasOne(sr => sr.PredefinedText)
            .WithMany()
            .HasForeignKey(sr => sr.PredefinedTextId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(sr => sr.Booking)
            .WithOne(b => b.ServiceRating)
            .HasForeignKey<ServiceRating>(sr => sr.BookingId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(sr => sr.ETicket)
            .WithOne(e => e.ServiceRating)
            .HasForeignKey<ServiceRating>(sr => sr.ETicketId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(sr => sr.Service)
            .WithMany()
            .HasForeignKey(sr => sr.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(sr => sr.User)
            .WithMany(u => u.ServiceRatings)
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
