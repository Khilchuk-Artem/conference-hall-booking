namespace ConferenceBooking.Application.DTO;

public class BookingServiceDto
{
    public Guid AdditionalServiceId { get; set; }
    public string ServiceName {  get; set; }
    public decimal Price {  get; set; }
}