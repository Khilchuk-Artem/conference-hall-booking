using System.Reflection;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data;

public class ConferenceBookingDbContext : DbContext
{
    public ConferenceBookingDbContext(DbContextOptions<ConferenceBookingDbContext> options) : base(options) { }

    public DbSet<ConferenceHall> ConferenceHalls { get; set; }
    public DbSet<AdditionalService> AdditionalServices { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingService> BookingServices { get; set; }
    public DbSet<IdempotencyKey> IdempotencyKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<BaseEntity>()
            .HasKey(entity => entity.Id);

        builder.Entity<ConferenceHall>();
        builder.Entity<AdditionalService>();
        builder.Entity<Booking>();
        builder.Entity<BookingService>();
        builder.Entity<IdempotencyKey>();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<BaseEntity>()
            .UseTpcMappingStrategy()
            .HasQueryFilter(entity => !entity.IsDeleted);

        base.OnModelCreating(builder);
    }
}
