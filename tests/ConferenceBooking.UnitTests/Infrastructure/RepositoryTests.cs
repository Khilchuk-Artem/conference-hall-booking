using ConferenceBooking.Application.Specifications.ConferenceHalls;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Infrastructure.Data;
using Xunit;

namespace ConferenceBooking.UnitTests.Infrastructure;

public class RepositoryTests : IClassFixture<TestPostgresContainer>
{
    private readonly TestPostgresContainer _container;

    public RepositoryTests(TestPostgresContainer container)
    {
        _container = container;
    }

    [Fact]
    public async Task Repository_CreatesReadsAndUpdatesEntity()
    {
        await using var context = _container.CreateContext();
        var repository = new Repository<ConferenceHall>(context);
        var hall = new ConferenceHall
        {
            Id = Guid.NewGuid(),
            Name = "Test Hall",
            Capacity = 80,
            RentRate = 1800m,
            AdditionalServices = [],
            Bookings = []
        };

        var added = await repository.Add(hall);
        var found = await repository.GetById(hall.Id);
        var foundWithServices = await repository.GetById(hall.Id, new ConferenceHallWithServicesSpecification());
        var all = await repository.GetAll(new ConferenceHallWithServicesSpecification());

        Assert.Same(hall, added);
        Assert.Equal("Test Hall", found!.Name);
        Assert.Equal(hall.Id, foundWithServices!.Id);
        Assert.Contains(all, item => item.Id == hall.Id);

        hall.Name = "Updated Hall";
        var updated = await repository.Update(hall);

        Assert.Same(hall, updated);
        Assert.Equal("Updated Hall", (await repository.GetById(hall.Id))!.Name);
    }

    [Fact]
    public async Task Repository_DeleteMarksEntityAndReturnsNullForMissingEntity()
    {
        await using var context = _container.CreateContext();
        var repository = new Repository<ConferenceHall>(context);
        var hall = new ConferenceHall
        {
            Id = Guid.NewGuid(),
            Name = "Test Hall",
            Capacity = 80,
            RentRate = 1800m,
            AdditionalServices = [],
            Bookings = []
        };
        await repository.Add(hall);

        var deleted = await repository.Delete(hall.Id);
        var missing = await repository.Delete(Guid.NewGuid());

        Assert.NotNull(deleted);
        Assert.True(deleted!.IsDeleted);
        Assert.Null(await repository.GetById(hall.Id));
        Assert.Null(missing);
    }
}
