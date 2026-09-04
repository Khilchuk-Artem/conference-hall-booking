namespace ConferenceBooking.Application.DTO;

public class ConferenceHallDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public decimal RentRate { get; set; }
    public List<AdditionalServiceDto> AdditionalServices { get; set; }
}