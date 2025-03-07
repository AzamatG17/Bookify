using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal class ETicketConfiguration : IEntityTypeConfiguration<ETicket>
{
    public void Configure(EntityTypeBuilder<ETicket> builder)
    {
        builder.ToTable(nameof(ETicket));
        builder.HasKey(c => c.Id);

        builder.Property(e => e.ETicketId);

        builder.Property(e => e.TicketId)
            .IsRequired();

        builder.Property(e => e.ServiceId)
            .IsRequired();

        builder.Property(e => e.Language)
            .HasMaxLength(10);

        builder.Property(e => e.ServiceName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.BranchName)
            .HasMaxLength(200);

        builder.Property(e => e.CreatedTime)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Message)
            .HasMaxLength(500);

        builder.Property(e => e.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.ValidUntil)
            .HasMaxLength(100);

        builder.Property(e => e.Success)
            .IsRequired();

        builder.Property(e => e.ShowArriveButton)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.HasOne(b => b.User)
            .WithMany(u => u.ETickets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
