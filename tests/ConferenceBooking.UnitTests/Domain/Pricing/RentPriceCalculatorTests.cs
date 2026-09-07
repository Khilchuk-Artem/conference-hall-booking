using ConferenceBooking.Domain.Pricing;
using Xunit;
using ConferenceBooking.UnitTests.TestData;

namespace ConferenceBooking.UnitTests.Domain.Pricing;

public class RentPriceCalculatorTests
{
    private readonly RentPriceCalculator _calculator = new();

    [Theory]
    [InlineData(6, 9, 5400)]
    [InlineData(9, 12, 6000)]
    [InlineData(12, 14, 4600)]
    [InlineData(14, 18, 8000)]
    [InlineData(18, 23, 8000)]
    public void CalculateTotalPrice_SinglePricingBracket_ReturnsExpectedPrice(int startHour, int endHour, decimal expectedPrice)
    {
        var result = _calculator.CalculateTotalPrice(2000, TestTimes.At(startHour), TestTimes.At(endHour));

        Assert.Equal(expectedPrice, result);
    }

    [Theory]
    [InlineData(10, 15, 10600)]
    [InlineData(8, 19, 22000)]
    [InlineData(6, 23, 32000)]
    public void CalculateTotalPrice_SpansMultiplePricingBrackets_ReturnsCombinedPrice(int startHour, int endHour, decimal expectedPrice)
    {
        var result = _calculator.CalculateTotalPrice(2000, TestTimes.At(startHour), TestTimes.At(endHour));

        Assert.Equal(expectedPrice, result);
    }

    [Fact]
    public void CalculateTotalPrice_PartialHours_ReturnsProportionalPrice()
    {
        var result = _calculator.CalculateTotalPrice(2000, TestTimes.At(8, 30), TestTimes.At(9, 30));

        Assert.Equal(1900, result);
    }

    [Fact]
    public void CalculateTotalPrice_WithPositiveOffset_UsesLocalBookingTime()
    {
        var result = _calculator.CalculateTotalPrice(
            2000,
            TestTimes.At(10, offsetHours: 3),
            TestTimes.At(14, offsetHours: 3));

        Assert.Equal(8600, result);
    }

    [Theory]
    [InlineData(5, 0, 6, 0)]
    [InlineData(22, 0, 23, 30)]
    public void CalculateTotalPrice_OutsideOperatingHours_ReturnsNull(int startHour, int startMinute, int endHour, int endMinute)
    {
        var result = _calculator.CalculateTotalPrice(2000, TestTimes.At(startHour, startMinute), TestTimes.At(endHour, endMinute));

        Assert.Null(result);
    }

    [Fact]
    public void CalculateTotalPrice_WhenEndPrecedesStart_ReturnsNull()
    {
        var result = _calculator.CalculateTotalPrice(2000, TestTimes.At(14), TestTimes.At(12));

        Assert.Null(result);
    }

    [Fact]
    public void CalculateTotalPrice_WhenBookingCrossesCalendarDay_ReturnsNull()
    {
        var result = _calculator.CalculateTotalPrice(
            2000,
            TestTimes.At(22, offsetHours: 3),
            TestTimes.NextDayAt(6, offsetHours: 3));

        Assert.Null(result);
    }
}
