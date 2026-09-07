namespace ConferenceBooking.Application.DTO;

public class AdditionalServiceReportDto
{
    public Guid AdditionalServiceId { get; set; }
    public string ServiceName { get; set; }
    public int UsageCount { get; set; }
    public decimal Revenue { get; set; }
}
