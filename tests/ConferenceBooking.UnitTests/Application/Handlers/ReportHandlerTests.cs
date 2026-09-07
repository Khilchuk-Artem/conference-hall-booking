using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Reports.GetSummary;
using ConferenceBooking.Application.Reports.Models;
using Moq;
using Xunit;
using ConferenceBooking.UnitTests.TestData;

namespace ConferenceBooking.UnitTests.Application.Handlers;

public class ReportHandlerTests
{
    [Fact]
    public async Task GetReportSummaryHandler_NormalizesPeriodAndMapsReport()
    {
        var from = TestTimes.At(10, offsetHours: 3);
        var to = TestTimes.NextDayAt(10, offsetHours: 3);
        DateTimeOffset? capturedFrom = null;
        DateTimeOffset? capturedTo = null;
        var repository = new Mock<IReportRepository>();
        repository
            .Setup(item => item.GetSummary(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .Callback<DateTimeOffset, DateTimeOffset>((requestFrom, requestTo) =>
            {
                capturedFrom = requestFrom;
                capturedTo = requestTo;
            })
            .ReturnsAsync(new ReportSummary
            {
                From = from.ToUniversalTime(),
                To = to.ToUniversalTime(),
                BookingsCount = 2,
                TotalHours = 6,
                TotalRevenue = 12000m,
                Halls = [new HallReport
                {
                    ConferenceHallId = Guid.NewGuid(),
                    HallName = "Hall A",
                    BookingsCount = 2,
                    TotalHours = 6,
                    Revenue = 12000m
                }],
                MostPopularAdditionalServices = [new AdditionalServiceReport
                {
                    AdditionalServiceId = Guid.NewGuid(),
                    ServiceName = "Projector",
                    UsageCount = 2,
                    Revenue = 200m
                }]
            });

        var result = await new GetReportSummaryHandler(repository.Object)
            .Handle(new GetReportSummaryQuery { From = from, To = to }, CancellationToken.None);

        Assert.Equal(from.ToUniversalTime(), capturedFrom);
        Assert.Equal(to.ToUniversalTime(), capturedTo);
        Assert.Equal(2, result.BookingsCount);
        Assert.Equal(6, result.TotalHours);
        Assert.Equal(12000m, result.TotalRevenue);
        Assert.Single(result.Halls);
        Assert.Single(result.MostPopularAdditionalServices);
    }
}
