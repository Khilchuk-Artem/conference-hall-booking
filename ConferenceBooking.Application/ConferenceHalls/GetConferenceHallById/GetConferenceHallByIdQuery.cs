using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.GetConferenceHallById;

public class GetConferenceHallByIdQuery : IRequest<ConferenceHallDto>
{
    public Guid Id { get; set; }
}
