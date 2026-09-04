using System.Reflection;
using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data;

public class ConferenceBookingDbContext : DbContext
{
    public ConferenceBookingDbContext(DbContextOptions<ConferenceBookingDbContext> options) : base(options) {}
    
    public DbSet<ConferenceHall> ConferenceHalls { get; set; }
    public DbSet<AdditionalService> AdditionalServices { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingService> BookingServices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        builder.Entity<BaseEntity>()
            .UseTpcMappingStrategy()
            .HasQueryFilter(entity => !entity.IsDeleted);
        
        base.OnModelCreating(builder);
    }
}