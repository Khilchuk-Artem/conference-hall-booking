using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.EditConferenceHall;

public class EditConferenceHallCommand :IRequest<ConferenceHallDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public decimal RentRate { get; set; }
    public List<Guid> AdditionalServiceIds { get; set; }
}