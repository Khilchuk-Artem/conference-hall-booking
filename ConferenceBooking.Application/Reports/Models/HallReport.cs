namespace ConferenceBooking.Application.Reports.Models;

public class HallReport
{
    public Guid ConferenceHallId { get; set; }
    public string HallName { get; set; }
    public int BookingsCount { get; set; }
    public double TotalHours { get; set; }
    public decimal Revenue { get; set; }
}
