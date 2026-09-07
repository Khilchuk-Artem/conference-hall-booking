using Ardalis.Specification;
using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.ConferenceHalls.CreateConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.DeleteConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.EditConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.GetAvailableConferenceHalls;
using ConferenceBooking.Application.ConferenceHalls.GetConferenceHallById;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Domain.Entities;
using Moq;
using Xunit;
using ConferenceBooking.UnitTests.TestData;

namespace ConferenceBooking.UnitTests.Application.Handlers;

public class ConferenceHallHandlersTests
{
    [Fact]
    public async Task CreateHallHandler_ValidRequest_CreatesAndReturnsId()
    {
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        var serviceRepository = new Mock<IRepository<AdditionalService>>();
        var service = ConferenceHallTestData.ProjectorService();
        serviceRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<AdditionalService>>()))
            .ReturnsAsync([service]);
        hallRepository
            .Setup(repository => repository.Add(It.IsAny<ConferenceHall>()))
            .ReturnsAsync((ConferenceHall hall) =>
            {
                hall.Id = TestIds.HallId;
                return hall;
            });

        var result = await new CreateConferenceHallHandler(hallRepository.Object, serviceRepository.Object)
            .Handle(ConferenceHallTestData.ValidCommand(), CancellationToken.None);

        Assert.Equal(TestIds.HallId, result);
        hallRepository.Verify(repository => repository.Add(It.Is<ConferenceHall>(hall =>
            hall.Name == "Hall A" && hall.Capacity == 100 && hall.RentRate == 2000m &&
            hall.AdditionalServices.Single().Id == TestIds.ServiceId)), Times.Once);
    }

    [Fact]
    public async Task CreateHallHandler_UnknownService_ThrowsBadRequest()
    {
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        var serviceRepository = new Mock<IRepository<AdditionalService>>();
        serviceRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<AdditionalService>>()))
            .ReturnsAsync([]);

        await Assert.ThrowsAsync<BadRequestException>(() => new CreateConferenceHallHandler(
                hallRepository.Object,
                serviceRepository.Object)
            .Handle(ConferenceHallTestData.ValidCommand(), CancellationToken.None));

        hallRepository.Verify(repository => repository.Add(It.IsAny<ConferenceHall>()), Times.Never);
    }

    [Fact]
    public async Task GetHallByIdHandler_ExistingHall_ReturnsDto()
    {
        var hall = ConferenceHallTestData.Hall(false);
        var repository = new Mock<IRepository<ConferenceHall>>();
        repository
            .Setup(item => item.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync(hall);

        var result = await new GetConferenceHallByIdHandler(repository.Object)
            .Handle(new GetConferenceHallByIdQuery { Id = TestIds.HallId }, CancellationToken.None);

        Assert.Equal(TestIds.HallId, result.Id);
        Assert.Equal("Hall A", result.Name);
        Assert.Single(result.AdditionalServices);
    }

    [Fact]
    public async Task GetHallByIdHandler_MissingHall_ThrowsNotFound()
    {
        var repository = new Mock<IRepository<ConferenceHall>>();
        repository
            .Setup(item => item.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync((ConferenceHall)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => new GetConferenceHallByIdHandler(repository.Object)
            .Handle(new GetConferenceHallByIdQuery { Id = TestIds.HallId }, CancellationToken.None));
    }

    [Fact]
    public async Task EditHallHandler_ValidRequest_UpdatesHallAndReturnsDto()
    {
        var hall = ConferenceHallTestData.Hall();
        var replacementService = ConferenceHallTestData.WiFiService();
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        var serviceRepository = new Mock<IRepository<AdditionalService>>();
        hallRepository
            .Setup(repository => repository.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync(hall);
        serviceRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<AdditionalService>>()))
            .ReturnsAsync([replacementService]);
        hallRepository
            .Setup(repository => repository.Update(hall))
            .ReturnsAsync(hall);

        var command = ConferenceHallTestData.ValidEditCommand(TestIds.SecondServiceId);
        command.Name = "Updated Hall";
        command.Capacity = 150;
        command.RentRate = 2500m;

        var result = await new EditConferenceHallHandler(hallRepository.Object, serviceRepository.Object)
            .Handle(command, CancellationToken.None);

        Assert.Equal("Updated Hall", result.Name);
        Assert.Equal(150, result.Capacity);
        Assert.Equal(2500m, result.RentRate);
        Assert.Equal(TestIds.SecondServiceId, result.AdditionalServices.Single().Id);
        hallRepository.Verify(repository => repository.Update(hall), Times.Once);
    }

    [Fact]
    public async Task EditHallHandler_MissingHall_ThrowsNotFound()
    {
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        var serviceRepository = new Mock<IRepository<AdditionalService>>();
        hallRepository
            .Setup(repository => repository.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync((ConferenceHall)null!);

        var command = ConferenceHallTestData.ValidEditCommand();

        await Assert.ThrowsAsync<NotFoundException>(() => new EditConferenceHallHandler(
                hallRepository.Object,
                serviceRepository.Object)
            .Handle(command, CancellationToken.None));

        serviceRepository.Verify(repository => repository.GetAll(It.IsAny<Specification<AdditionalService>>()), Times.Never);
    }

    [Fact]
    public async Task EditHallHandler_UnknownService_ThrowsBadRequest()
    {
        var hallRepository = new Mock<IRepository<ConferenceHall>>();
        var serviceRepository = new Mock<IRepository<AdditionalService>>();
        hallRepository
            .Setup(repository => repository.GetById(TestIds.HallId, It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync(ConferenceHallTestData.Hall());
        serviceRepository
            .Setup(repository => repository.GetAll(It.IsAny<Specification<AdditionalService>>()))
            .ReturnsAsync([]);

        var command = ConferenceHallTestData.ValidEditCommand();
        command.Name = "Updated Hall";
        command.Capacity = 150;
        command.RentRate = 2500m;
        command.AdditionalServiceIds = [TestIds.ServiceId];

        await Assert.ThrowsAsync<BadRequestException>(() => new EditConferenceHallHandler(
                hallRepository.Object,
                serviceRepository.Object)
            .Handle(command, CancellationToken.None));

        hallRepository.Verify(repository => repository.Update(It.IsAny<ConferenceHall>()), Times.Never);
    }

    [Fact]
    public async Task DeleteHallHandler_ExistingHall_ReturnsDeletedDto()
    {
        var hall = ConferenceHallTestData.Hall(false);
        var repository = new Mock<IRepository<ConferenceHall>>();
        repository.Setup(item => item.GetById(TestIds.HallId)).ReturnsAsync(hall);
        repository.Setup(item => item.Delete(TestIds.HallId)).ReturnsAsync(hall);

        var result = await new DeleteConferenceHallHandler(repository.Object)
            .Handle(new DeleteConferenceHallCommand { Id = TestIds.HallId }, CancellationToken.None);

        Assert.Equal(TestIds.HallId, result.Id);
        Assert.Equal("Hall A", result.Name);
        repository.Verify(item => item.Delete(TestIds.HallId), Times.Once);
    }

    [Fact]
    public async Task DeleteHallHandler_MissingHall_ThrowsNotFoundWithoutDelete()
    {
        var repository = new Mock<IRepository<ConferenceHall>>();
        repository.Setup(item => item.GetById(TestIds.HallId)).ReturnsAsync((ConferenceHall)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => new DeleteConferenceHallHandler(repository.Object)
            .Handle(new DeleteConferenceHallCommand { Id = TestIds.HallId }, CancellationToken.None));

        repository.Verify(item => item.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeleteHallHandler_DeleteReturnsNull_ThrowsNotFound()
    {
        var repository = new Mock<IRepository<ConferenceHall>>();
        repository.Setup(item => item.GetById(TestIds.HallId)).ReturnsAsync(ConferenceHallTestData.Hall(false));
        repository.Setup(item => item.Delete(TestIds.HallId)).ReturnsAsync((ConferenceHall)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => new DeleteConferenceHallHandler(repository.Object)
            .Handle(new DeleteConferenceHallCommand { Id = TestIds.HallId }, CancellationToken.None));
    }

    [Fact]
    public async Task GetAvailableHallsHandler_ReturnsMappedHalls()
    {
        var repository = new Mock<IRepository<ConferenceHall>>();
        repository
            .Setup(item => item.GetAll(It.IsAny<Specification<ConferenceHall>>()))
            .ReturnsAsync([ConferenceHallTestData.Hall(false)]);

        var result = await new GetAvailableConferenceHallHandler(repository.Object)
            .Handle(new GetAvailableConferenceHallsQuery
            {
                Capacity = 50,
                StartTime = TestTimes.At(10, offsetHours: 3),
                EndTime = TestTimes.At(12, offsetHours: 3),
                Page = 1,
                PageSize = 10
            }, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(TestIds.HallId, result[0].Id);
        Assert.Equal("Hall A", result[0].Name);
    }
}
