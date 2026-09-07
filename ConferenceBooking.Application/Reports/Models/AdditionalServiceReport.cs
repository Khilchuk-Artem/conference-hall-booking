namespace ConferenceBooking.Application.Reports.Models;

public class AdditionalServiceReport
{
    public Guid AdditionalServiceId { get; set; }
    public string ServiceName { get; set; }
    public int UsageCount { get; set; }
    public decimal Revenue { get; set; }
}
