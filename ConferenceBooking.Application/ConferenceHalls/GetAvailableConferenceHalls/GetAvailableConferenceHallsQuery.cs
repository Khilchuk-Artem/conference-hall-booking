using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.GetAvailableConferenceHalls;

public class GetAvailableConferenceHallsQuery : IRequest<List<ConferenceHallDto>>
{
    public int Capacity { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}