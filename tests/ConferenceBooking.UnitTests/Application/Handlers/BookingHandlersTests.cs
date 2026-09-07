using Ardalis.Specification;
using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Bookings.CreateBooking;
using ConferenceBooking.Application.Bookings.GetBookingById;
using ConferenceBooking.Application.Bookings.GetBookings;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Domain.Pricing;
using Moq;
using Xunit;
using ConferenceBooking.UnitTests.TestData;

namespace ConferenceBooking.UnitTests.Application.Handlers;

public class BookingHandlersTests
{
    [Fact]
    public async Task CreateBookingHandler_ValidRequest_CreatesAndReturnsBooking()
    {
        var bookingRepository = new Mock<IRepository<Booking>>();
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        var hall = ConferenceHallTestData.Hall();
        var start = TestTimes.At(10, offsetHours: 3);
        var end = TestTimes.At(14, offsetHours: 3);
        var command = BookingTestData.ValidCommand(TestIds.HallId, TestIds.ServiceId, TestIds.SecondServiceId);
        command.StartTime = start;
        command.EndTime = end;

        bookingRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<Booking>>()))
            .ReturnsAsync([]);
        hallRepository
            .Setup(repository => repository.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync(hall);
        bookingRepository
            .Setup(repository => repository.Add(It.IsAny<Booking>()))
            .ReturnsAsync((Booking booking) =>
            {
                booking.Id = TestIds.BookingId;
                return booking;
            });

        var result = await new CreateBookingHandler(bookingRepository.Object, hallRepository.Object, new RentPriceCalculator())
            .Handle(command, CancellationToken.None);

        Assert.Equal(TestIds.BookingId, result.Id);
        Assert.Equal(TestIds.HallId, result.ConferenceHallId);
        Assert.Equal(start.ToUniversalTime(), result.StartTime);
        Assert.Equal(end.ToUniversalTime(), result.EndTime);
        Assert.Equal(8600m, result.HallCost);
        Assert.Equal(350m, result.TotalServicesCost);
        Assert.Equal(8950m, result.TotalCost);
        Assert.Equal(2, result.AdditionalServices.Count);
        bookingRepository.Verify(repository => repository.Add(It.Is<Booking>(booking =>
            booking.StartTime == start.ToUniversalTime() && booking.EndTime == end.ToUniversalTime())), Times.Once);
    }

    [Fact]
    public async Task CreateBookingHandler_OverlappingBooking_ThrowsConflict()
    {
        var bookingRepository = new Mock<IRepository<Booking>>();
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        bookingRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<Booking>>()))
            .ReturnsAsync([new Booking()]);

        var exception = await Assert.ThrowsAsync<ConflictException>(() => new CreateBookingHandler(
                bookingRepository.Object,
                hallRepository.Object,
                new RentPriceCalculator())
            .Handle(BookingTestData.ValidCommand(), CancellationToken.None));

        Assert.Contains("already booked", exception.Message);
        hallRepository.Verify(repository => repository.GetById(
            It.IsAny<Guid>(), It.IsAny<Specification<ConferenceHall>>()), Times.Never);
    }

    [Fact]
    public async Task CreateBookingHandler_MissingHall_ThrowsNotFound()
    {
        var bookingRepository = new Mock<IRepository<Booking>>();
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        bookingRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<Booking>>()))
            .ReturnsAsync([]);
        hallRepository
            .Setup(repository => repository.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync((ConferenceHall)null!);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => new CreateBookingHandler(
                bookingRepository.Object,
                hallRepository.Object,
                new RentPriceCalculator())
            .Handle(BookingTestData.ValidCommand(), CancellationToken.None));

        Assert.Contains("Conference hall", exception.Message);
        bookingRepository.Verify(repository => repository.Add(It.IsAny<Booking>()), Times.Never);
    }

    [Fact]
    public async Task CreateBookingHandler_UnknownService_ThrowsBadRequest()
    {
        var bookingRepository = new Mock<IRepository<Booking>>();
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        var hall = ConferenceHallTestData.Hall();
        bookingRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<Booking>>()))
            .ReturnsAsync([]);
        hallRepository
            .Setup(repository => repository.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync(hall);

        var command = BookingTestData.ValidCommand(TestIds.HallId, TestIds.ServiceId, Guid.NewGuid());

        await Assert.ThrowsAsync<BadRequestException>(() => new CreateBookingHandler(
                bookingRepository.Object,
                hallRepository.Object,
                new RentPriceCalculator())
            .Handle(command, CancellationToken.None));

        bookingRepository.Verify(repository => repository.Add(It.IsAny<Booking>()), Times.Never);
    }

    [Fact]
    public async Task GetBookingByIdHandler_ExistingBooking_ReturnsDto()
    {
        var bookingId = Guid.NewGuid();
        var booking = BookingTestData.Existing(bookingId);
        booking.AdditionalServices = [new BookingService
        {
            AdditionalServiceId = TestIds.ServiceId,
            ServiceName = "Projector",
            Price = 100m
        }];
        var repository = new Mock<IRepository<Booking>>();
        repository
            .Setup(item => item.GetById(bookingId, It.IsAny<Specification<Booking>>()))
            .ReturnsAsync(booking);

        var result = await new GetBookingByIdHandler(repository.Object)
            .Handle(new GetBookingByIdQuery { Id = bookingId }, CancellationToken.None);

        Assert.Equal(bookingId, result.Id);
        Assert.Equal("Hall A", result.HallName);
        Assert.Single(result.AdditionalServices);
    }

    [Fact]
    public async Task GetBookingByIdHandler_MissingBooking_ThrowsNotFound()
    {
        var bookingId = Guid.NewGuid();
        var repository = new Mock<IRepository<Booking>>();
        repository
            .Setup(item => item.GetById(bookingId, It.IsAny<Specification<Booking>>()))
            .ReturnsAsync((Booking)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => new GetBookingByIdHandler(repository.Object)
            .Handle(new GetBookingByIdQuery { Id = bookingId }, CancellationToken.None));
    }

    [Fact]
    public async Task GetBookingsHandler_ReturnsMappedBookings()
    {
        var repository = new Mock<IRepository<Booking>>();
        repository
            .Setup(item => item.GetAll(It.IsAny<Specification<Booking>>()))
            .ReturnsAsync([
                new Booking { Id = Guid.NewGuid(), HallName = "Hall A", AdditionalServices = [] },
                new Booking { Id = Guid.NewGuid(), HallName = "Hall B", AdditionalServices = [] }
            ]);

        var result = await new GetBookingsHandler(repository.Object)
            .Handle(new GetBookingsQuery { Page = 1, PageSize = 10 }, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Hall A", result[0].HallName);
        Assert.Equal("Hall B", result[1].HallName);
    }

}
