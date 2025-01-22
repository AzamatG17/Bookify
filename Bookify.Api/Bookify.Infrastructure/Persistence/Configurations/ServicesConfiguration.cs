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

        builder.HasOne(s => s.Branch)
            .WithMany(b => b.Services)
            .HasForeignKey(s => s.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
