using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Infrastructure.Data;
using ConferenceBooking.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace ConferenceBooking.UnitTests.Infrastructure;

public class ConferenceBookingDbContextModelTests
{
    [Fact]
    public void Model_ContainsConfiguredTablesAndSeedData()
    {
        using var context = TestDbContextFactory.Create();
        var model = context.Model;

        var halls = model.FindEntityType(typeof(ConferenceHall))!;
        var services = model.FindEntityType(typeof(AdditionalService))!;
        var bookings = model.FindEntityType(typeof(Booking))!;
        var idempotencyKeys = model.FindEntityType(typeof(IdempotencyKey))!;
        var hallServices = model.GetEntityTypes().Single(entity => entity.GetTableName() == "ConferenceHallAdditionalServices");

        Assert.Equal("ConferenceHalls", halls!.GetTableName());
        Assert.Equal("AdditionalServices", services!.GetTableName());
        Assert.Equal("Bookings", bookings!.GetTableName());
        Assert.Equal("IdempotencyKeys", idempotencyKeys!.GetTableName());
        var designModel = context.GetService<IDesignTimeModel>().Model;
        Assert.Equal(3, designModel.FindEntityType(typeof(ConferenceHall))!.GetSeedData().Count());
        Assert.Equal(3, designModel.FindEntityType(typeof(AdditionalService))!.GetSeedData().Count());
        Assert.Equal(9, designModel.GetEntityTypes().Single(entity => entity.GetTableName() == "ConferenceHallAdditionalServices").GetSeedData().Count());
    }

    [Fact]
    public void Model_UsesUtcTimestampAndIdempotencyIndexes()
    {
        using var context = TestDbContextFactory.Create();
        var booking = context.Model.FindEntityType(typeof(Booking))!;
        var idempotency = context.Model.FindEntityType(typeof(IdempotencyKey))!;

        Assert.Equal("timestamp with time zone", booking.FindProperty(nameof(Booking.StartTime))!.GetColumnType());
        Assert.Equal("timestamp with time zone", booking.FindProperty(nameof(Booking.EndTime))!.GetColumnType());
        Assert.Contains(idempotency.GetIndexes(), index =>
            index.Properties.Select(property => property.Name).SequenceEqual([nameof(IdempotencyKey.Operation), nameof(IdempotencyKey.Key)]));
        Assert.Contains(idempotency.GetIndexes(), index =>
            index.Properties.Single().Name == nameof(IdempotencyKey.ExpiresAt));
    }

}
