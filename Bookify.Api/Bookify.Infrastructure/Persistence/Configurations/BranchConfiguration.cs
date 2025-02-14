using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable(nameof(Branch));
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.BranchAddres);

        builder.Property(c => c.CoordinateLatitude);

        builder.Property(c => c.CoordinateLongitude);

        builder.HasOne(c => c.Companies)
            .WithMany(b => b.Branches)
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.OpeningTimeBranches)
            .WithOne()
            .HasForeignKey("BranchId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Services)
            .WithOne(b => b.Branch)
            .HasForeignKey(b => b.BranchId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
