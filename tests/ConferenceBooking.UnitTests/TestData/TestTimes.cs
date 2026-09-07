namespace ConferenceBooking.UnitTests.TestData;

internal static class TestTimes
{
    internal static DateTimeOffset At(int hour, int minute = 0, int offsetHours = 0) =>
        new(2030, 1, 1, hour, minute, 0, TimeSpan.FromHours(offsetHours));

    internal static DateTimeOffset NextDayAt(int hour, int minute = 0, int offsetHours = 0) =>
        new(2030, 1, 2, hour, minute, 0, TimeSpan.FromHours(offsetHours));
}
