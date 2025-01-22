using Bookify.Domain_.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal class OpeningTimeBranchConfiguration : IEntityTypeConfiguration<OpeningTimeBranch>
{
    public void Configure(EntityTypeBuilder<OpeningTimeBranch> builder)
    {
        builder.ToTable(nameof(OpeningTimeBranch));
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Day)
            .IsRequired();

        builder.Property(o => o.OpenTime)
            .HasMaxLength(30);
    }
}
