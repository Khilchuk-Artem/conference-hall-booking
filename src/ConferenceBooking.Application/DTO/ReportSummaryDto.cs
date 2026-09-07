namespace ConferenceBooking.Application.DTO;

public class ReportSummaryDto
{
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public int BookingsCount { get; set; }
    public double TotalHours { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<HallReportDto> Halls { get; set; }
    public List<AdditionalServiceReportDto> MostPopularAdditionalServices { get; set; }
}
