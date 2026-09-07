using ConferenceBooking.Application.AdditionalServices.CreateAdditionalService;
using ConferenceBooking.Application.AdditionalServices.DeleteAdditionalService;
using ConferenceBooking.Application.AdditionalServices.EditAdditionalService;
using ConferenceBooking.Application.AdditionalServices.GetAdditionalServices;
using ConferenceBooking.UnitTests.TestData;
using Xunit;

namespace ConferenceBooking.UnitTests.Application.Validation;

public class AdditionalServiceValidatorsTests
{
    [Fact]
    public void CreateAdditionalServiceValidator_ValidRequest_Passes()
    {
        var result = new CreateAdditionalServiceCommandValidator()
            .Validate(AdditionalServiceTestData.ValidCreateCommand());

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateAdditionalServiceValidator_NonPositivePrice_IsRejected(decimal price)
    {
        var command = AdditionalServiceTestData.ValidCreateCommand();
        command.Price = price;

        var result = new CreateAdditionalServiceCommandValidator().Validate(command);

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(command.Price));
    }

    [Fact]
    public void EditAdditionalServiceValidator_RequiresIdAndValidFields()
    {
        var result = new EditAdditionalServiceCommandValidator().Validate(new EditAdditionalServiceCommand
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Price = 0
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditAdditionalServiceCommand.Id));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditAdditionalServiceCommand.Name));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditAdditionalServiceCommand.Price));
    }

    [Fact]
    public void DeleteAdditionalServiceValidator_RequiresId()
    {
        var result = new DeleteAdditionalServiceCommandValidator()
            .Validate(new DeleteAdditionalServiceCommand());

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(DeleteAdditionalServiceCommand.Id));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    public void GetAdditionalServicesValidator_RequiresPositivePaging(int page, int pageSize)
    {
        var result = new GetAdditionalServicesQueryValidator()
            .Validate(new GetAdditionalServicesQuery { Page = page, PageSize = pageSize });

        Assert.False(result.IsValid);
    }
}
