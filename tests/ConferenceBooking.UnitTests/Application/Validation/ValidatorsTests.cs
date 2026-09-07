using ConferenceBooking.Application.Bookings.CreateBooking;
using ConferenceBooking.Application.Bookings.GetBookings;
using ConferenceBooking.Application.ConferenceHalls.CreateConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.DeleteConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.EditConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.GetAvailableConferenceHalls;
using ConferenceBooking.Application.Reports.GetSummary;
using FluentValidation;
using Xunit;
using ConferenceBooking.UnitTests.TestData;

namespace ConferenceBooking.UnitTests.Application.Validation;

public class ValidatorsTests
{
    [Fact]
    public void CreateBookingValidator_ValidRequest_Passes()
    {
        var result = new CreateBookingCommandValidator().Validate(BookingTestData.ValidCommand(Guid.NewGuid()));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateBookingValidator_InvalidRequest_ReturnsExpectedErrors()
    {
        var result = new CreateBookingCommandValidator().Validate(new CreateBookingCommand
        {
            ConferenceHallId = Guid.Empty,
            StartTime = TestTimes.At(23),
            EndTime = TestTimes.At(5),
            AdditionalServiceIds = [TestIds.ServiceId, TestIds.ServiceId]
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateBookingCommand.ConferenceHallId));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateBookingCommand.EndTime));
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("Duplicate"));
    }

    [Fact]
    public void CreateBookingValidator_OutsideOperatingHours_IsRejected()
    {
        var command = BookingTestData.ValidCommand(Guid.NewGuid());
        command.StartTime = TestTimes.At(5);
        command.EndTime = TestTimes.At(6);

        var result = new CreateBookingCommandValidator().Validate(command);

        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("within"));
    }

    [Fact]
    public void CreateBookingValidator_CrossDayRequest_IsRejected()
    {
        var command = BookingTestData.ValidCommand(Guid.NewGuid());
        command.StartTime = TestTimes.At(22);
        command.EndTime = TestTimes.NextDayAt(6);

        var result = new CreateBookingCommandValidator().Validate(command);

        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("calendar day"));
    }

    [Fact]
    public void CreateBookingValidator_NullServices_IsRejected()
    {
        var command = BookingTestData.ValidCommand(Guid.NewGuid());
        command.AdditionalServiceIds = null!;

        var result = new CreateBookingCommandValidator().Validate(command);

        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateBookingCommand.AdditionalServiceIds));
    }

    [Fact]
    public void CreateHallValidator_ValidRequest_Passes()
    {
        var result = new CreateConferenceHallCommandValidator().Validate(ConferenceHallTestData.ValidCommand());

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(10, 0)]
    public void CreateHallValidator_NonPositiveCapacityOrRate_IsRejected(int capacity, decimal rentRate)
    {
        var command = ConferenceHallTestData.ValidCommand();
        command.Capacity = capacity;
        command.RentRate = rentRate;

        var result = new CreateConferenceHallCommandValidator().Validate(command);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateHallValidator_DuplicateServices_IsRejected()
    {
        var command = ConferenceHallTestData.ValidCommand();
        command.AdditionalServiceIds = [TestIds.ServiceId, TestIds.ServiceId];

        var result = new CreateConferenceHallCommandValidator().Validate(command);

        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("Duplicate"));
    }

    [Fact]
    public void EditHallValidator_RequiresIdAndValidFields()
    {
        var command = new EditConferenceHallCommand
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Capacity = 0,
            RentRate = 0,
            AdditionalServiceIds = null!
        };

        var result = new EditConferenceHallCommandValidator().Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditConferenceHallCommand.Id));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditConferenceHallCommand.Name));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditConferenceHallCommand.Capacity));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditConferenceHallCommand.RentRate));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(EditConferenceHallCommand.AdditionalServiceIds));
    }

    [Fact]
    public void DeleteHallValidator_RequiresId()
    {
        var result = new DeleteConferenceHallCommandValidator().Validate(new DeleteConferenceHallCommand());

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(DeleteConferenceHallCommand.Id));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    public void GetBookingsValidator_RequiresPositivePaging(int page, int pageSize)
    {
        var result = new GetBookingsQueryValidator().Validate(new GetBookingsQuery { Page = page, PageSize = pageSize });

        Assert.False(result.IsValid);
    }

    [Fact]
    public void AvailableHallsValidator_RejectsInvalidTimeAndPaging()
    {
        var result = new GetAvailableConferenceHallsQueryValidator().Validate(new GetAvailableConferenceHallsQuery
        {
            Capacity = 0,
            StartTime = TestTimes.At(12),
            EndTime = TestTimes.At(11),
            Page = 0,
            PageSize = 0
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAvailableConferenceHallsQuery.Capacity));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAvailableConferenceHallsQuery.EndTime));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAvailableConferenceHallsQuery.Page));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAvailableConferenceHallsQuery.PageSize));
    }

    [Fact]
    public void ReportValidator_RequiresEndAfterStart()
    {
        var from = TestTimes.At(12);
        var result = new GetReportSummaryQueryValidator().Validate(new GetReportSummaryQuery { From = from, To = from });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetReportSummaryQuery.To));
    }

}
