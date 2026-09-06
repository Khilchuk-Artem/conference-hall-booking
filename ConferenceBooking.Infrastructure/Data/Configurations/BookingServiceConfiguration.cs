using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Data.Configurations;

public class BookingServiceConfiguration : IEntityTypeConfiguration<BookingService>
{
    public void Configure(EntityTypeBuilder<BookingService> builder)
    {
        builder.ToTable("BookingServices");
        
        builder.Property(x => x.ServiceName)
            .IsRequired()
            .HasMaxLength(200);       
        
        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.Property(x => x.IsDeleted)
            .IsRequired();
        
        builder.HasOne(x => x.Booking)
            .WithMany(b => b.AdditionalServices)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.AdditionalService)
            .WithMany()
            .HasForeignKey(x => x.AdditionalServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.BookingId,
            x.AdditionalServiceId
        }).IsUnique();
    }
}
