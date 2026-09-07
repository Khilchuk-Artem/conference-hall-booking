using Ardalis.Specification;
using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.AdditionalServices.CreateAdditionalService;
using ConferenceBooking.Application.AdditionalServices.DeleteAdditionalService;
using ConferenceBooking.Application.AdditionalServices.EditAdditionalService;
using ConferenceBooking.Application.AdditionalServices.GetAdditionalServiceById;
using ConferenceBooking.Application.AdditionalServices.GetAdditionalServices;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Domain.Entities;
using Moq;
using Xunit;
using ConferenceBooking.UnitTests.TestData;

namespace ConferenceBooking.UnitTests.Application.Handlers;

public class AdditionalServiceHandlersTests
{
    [Fact]
    public async Task CreateAdditionalServiceHandler_ValidRequest_CreatesAndReturnsId()
    {
        var repository = new Mock<IRepository<AdditionalService>>();
        repository
            .Setup(item => item.Add(It.IsAny<AdditionalService>()))
            .ReturnsAsync((AdditionalService service) =>
            {
                service.Id = TestIds.ServiceId;
                return service;
            });

        var result = await new CreateAdditionalServiceHandler(repository.Object)
            .Handle(AdditionalServiceTestData.ValidCreateCommand(), CancellationToken.None);

        Assert.Equal(TestIds.ServiceId, result);
        repository.Verify(item => item.Add(It.Is<AdditionalService>(service =>
            service.Name == "Projector" && service.Price == 100m)), Times.Once);
    }

    [Fact]
    public async Task GetAdditionalServiceByIdHandler_ExistingService_ReturnsDto()
    {
        var repository = new Mock<IRepository<AdditionalService>>();
        repository.Setup(item => item.GetById(TestIds.ServiceId)).ReturnsAsync(AdditionalServiceTestData.Projector());

        var result = await new GetAdditionalServiceByIdHandler(repository.Object)
            .Handle(new GetAdditionalServiceByIdQuery { Id = TestIds.ServiceId }, CancellationToken.None);

        Assert.Equal(TestIds.ServiceId, result.Id);
        Assert.Equal("Projector", result.Name);
        Assert.Equal(100m, result.Price);
    }

    [Fact]
    public async Task GetAdditionalServiceByIdHandler_MissingService_ThrowsNotFound()
    {
        var repository = new Mock<IRepository<AdditionalService>>();
        repository.Setup(item => item.GetById(TestIds.ServiceId)).ReturnsAsync((AdditionalService)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => new GetAdditionalServiceByIdHandler(repository.Object)
            .Handle(new GetAdditionalServiceByIdQuery { Id = TestIds.ServiceId }, CancellationToken.None));
    }

    [Fact]
    public async Task GetAdditionalServicesHandler_ReturnsMappedServices()
    {
        var repository = new Mock<IRepository<AdditionalService>>();
        repository
            .Setup(item => item.GetAll(It.IsAny<Specification<AdditionalService>>()))
            .ReturnsAsync([AdditionalServiceTestData.Projector()]);

        var result = await new GetAdditionalServicesHandler(repository.Object)
            .Handle(new GetAdditionalServicesQuery { Page = 1, PageSize = 10 }, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(TestIds.ServiceId, result[0].Id);
        Assert.Equal("Projector", result[0].Name);
    }

    [Fact]
    public async Task EditAdditionalServiceHandler_ValidRequest_UpdatesAndReturnsDto()
    {
        var service = AdditionalServiceTestData.Projector();
        var repository = new Mock<IRepository<AdditionalService>>();
        repository.Setup(item => item.GetById(TestIds.ServiceId)).ReturnsAsync(service);
        repository.Setup(item => item.Update(service)).ReturnsAsync(service);

        var result = await new EditAdditionalServiceHandler(repository.Object)
            .Handle(AdditionalServiceTestData.ValidEditCommand(), CancellationToken.None);

        Assert.Equal("Updated Projector", result.Name);
        Assert.Equal(150m, result.Price);
        repository.Verify(item => item.Update(service), Times.Once);
    }

    [Fact]
    public async Task EditAdditionalServiceHandler_MissingService_ThrowsNotFound()
    {
        var repository = new Mock<IRepository<AdditionalService>>();
        repository.Setup(item => item.GetById(TestIds.ServiceId)).ReturnsAsync((AdditionalService)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => new EditAdditionalServiceHandler(repository.Object)
            .Handle(AdditionalServiceTestData.ValidEditCommand(), CancellationToken.None));

        repository.Verify(item => item.Update(It.IsAny<AdditionalService>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAdditionalServiceHandler_ExistingService_ReturnsDeletedDto()
    {
        var service = AdditionalServiceTestData.Projector();
        var repository = new Mock<IRepository<AdditionalService>>();
        repository.Setup(item => item.GetById(TestIds.ServiceId)).ReturnsAsync(service);
        repository.Setup(item => item.Delete(TestIds.ServiceId)).ReturnsAsync(service);

        var result = await new DeleteAdditionalServiceHandler(repository.Object)
            .Handle(new DeleteAdditionalServiceCommand { Id = TestIds.ServiceId }, CancellationToken.None);

        Assert.Equal(TestIds.ServiceId, result.Id);
        Assert.Equal("Projector", result.Name);
        repository.Verify(item => item.Delete(TestIds.ServiceId), Times.Once);
    }

    [Fact]
    public async Task DeleteAdditionalServiceHandler_MissingService_ThrowsNotFoundWithoutDelete()
    {
        var repository = new Mock<IRepository<AdditionalService>>();
        repository.Setup(item => item.GetById(TestIds.ServiceId)).ReturnsAsync((AdditionalService)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => new DeleteAdditionalServiceHandler(repository.Object)
            .Handle(new DeleteAdditionalServiceCommand { Id = TestIds.ServiceId }, CancellationToken.None));

        repository.Verify(item => item.Delete(It.IsAny<Guid>()), Times.Never);
    }
}
