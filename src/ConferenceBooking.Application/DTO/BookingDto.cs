namespace ConferenceBooking.Application.DTO;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid ConferenceHallId { get; set; }

    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }

    public string HallName {  get; set; }
    public decimal HallHourlyRate { get; set; }
        
    public decimal HallCost {  get; set; }
    public decimal TotalServicesCost { get; set; }
    public decimal TotalCost {  get; set; }

    public List<BookingServiceDto> AdditionalServices { get; set; }
}