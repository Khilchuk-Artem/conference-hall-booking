using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.DeleteConferenceHall;

public class DeleteConferenceHallCommand : IRequest<ConferenceHallDto>
{
    public Guid Id { get; set; }
}