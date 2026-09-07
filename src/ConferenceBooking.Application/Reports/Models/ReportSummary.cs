namespace ConferenceBooking.Application.Reports.Models;

public class ReportSummary
{
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public int BookingsCount { get; set; }
    public double TotalHours { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<HallReport> Halls { get; set; }
    public List<AdditionalServiceReport> MostPopularAdditionalServices { get; set; }
}
