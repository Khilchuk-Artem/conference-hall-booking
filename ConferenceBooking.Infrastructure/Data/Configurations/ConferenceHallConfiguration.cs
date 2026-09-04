using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Data.Configurations;

public class ConferenceHallConfiguration : IEntityTypeConfiguration<ConferenceHall>
{
    public void Configure(EntityTypeBuilder<ConferenceHall> builder)
    {
        builder.ToTable("ConferenceHalls");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.Capacity)
            .IsRequired();
        
        builder.Property(x => x.RentRate)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.IsDeleted)
            .IsRequired();
        
        builder.HasMany(x => x.AdditionalServices)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "ConferenceHallAdditionalService",
                serviceNavigation  => serviceNavigation 
                    .HasOne<AdditionalService>()
                    .WithMany()
                    .HasForeignKey("AdditionalServiceId")
                    .OnDelete(DeleteBehavior.Restrict),
                hallNavigation  => hallNavigation 
                    .HasOne<ConferenceHall>()
                    .WithMany()
                    .HasForeignKey("ConferenceHallId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("ConferenceHallId", "AdditionalServiceId");
                    join.ToTable("ConferenceHallAdditionalServices");
                });
    }
}