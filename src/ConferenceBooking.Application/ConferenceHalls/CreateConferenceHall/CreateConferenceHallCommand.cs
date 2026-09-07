using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.CreateConferenceHall;

public class CreateConferenceHallCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public decimal RentRate { get; set; }
    public List<Guid> AdditionalServiceIds { get; set; }
}