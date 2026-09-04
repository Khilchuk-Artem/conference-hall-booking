namespace ConferenceBooking.Domain.Pricing;

public class PricingBracket
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Multiplier { get; set; }
}