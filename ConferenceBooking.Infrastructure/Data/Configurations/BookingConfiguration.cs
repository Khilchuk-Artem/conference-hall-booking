using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.StartTime)
            .IsRequired();
        
        builder.Property(x => x.EndTime)
            .IsRequired();
        
        builder.Property(x => x.HallName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.HallHourlyRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.HallCost)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalServicesCost)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalCost)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.IsDeleted)
            .IsRequired();
        
        builder.HasOne(x => x.ConferenceHall)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.ConferenceHallId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.AdditionalServices)
            .WithOne(x => x.Booking)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}