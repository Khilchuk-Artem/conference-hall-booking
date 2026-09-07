namespace ConferenceBooking.Domain.Pricing;

public class RentPriceCalculator
{
    private static readonly PricingBracket[] Brackets =
    [
        new PricingBracket { StartTime = new TimeOnly(6, 0), EndTime = new TimeOnly(9, 0), Multiplier = 0.9m },
        new PricingBracket { StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0), Multiplier = 1m },
        new PricingBracket { StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(14, 0), Multiplier = 1.15m },
        new PricingBracket { StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(18, 0), Multiplier = 1m },
        new PricingBracket { StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(23, 0), Multiplier = 0.8m }
    ];
    
    public decimal? CalculateTotalPrice(decimal baseRent, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (startDate.Date != endDate.Date) return null;
        if (endDate <= startDate) return null;
        
        var startTime = TimeOnly.FromTimeSpan(startDate.TimeOfDay);
        var endTime = TimeOnly.FromTimeSpan(endDate.TimeOfDay);
        
        decimal total = 0;
        var current = startTime;

        if (startTime < new TimeOnly(6, 0) || endTime > new TimeOnly(23, 0)) return null;
        
        // apply each bracket's rate to the part of the booking it covers
        while (current < endTime)
        {
            var bracket = Brackets.First(x =>
                current >= x.StartTime &&
                current < x.EndTime);

            var segmentEnd = bracket.EndTime < endTime ? bracket.EndTime : endTime;

            var hours = (decimal)(segmentEnd - current).TotalMinutes / 60m;

            total += baseRent * hours * bracket.Multiplier;

            current = segmentEnd;
        }

        return total;
    }
}
