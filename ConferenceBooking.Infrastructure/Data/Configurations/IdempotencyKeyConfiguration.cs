using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Idempotency;

public sealed class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKey>
{
    public void Configure(EntityTypeBuilder<IdempotencyKey> builder)
    {
        builder.ToTable("IdempotencyKeys");

        builder.Property(key => key.Operation)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(key => key.Key)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(key => key.RequestHash)
            .IsRequired()
            .HasColumnType("bytea");
        builder.Property(key => key.ResponseBody)
            .IsRequired()
            .HasColumnType("bytea");
        builder.Property(key => key.ResponseContentType)
            .HasMaxLength(200);
        builder.Property(key => key.Location)
            .HasMaxLength(2000);
        builder.Property(key => key.ExpiresAt)
            .IsRequired();

        builder.HasIndex(key => new { key.Operation, key.Key }).IsUnique();
        builder.HasIndex(key => key.ExpiresAt);
    }
}
