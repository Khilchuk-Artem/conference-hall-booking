using MediatR;

namespace ConferenceBooking.Application.Bookings.CreateBooking;

public class CreateBookingCommand : IRequest<Guid>
{
    public Guid ConferenceHallId { get; set; }

    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    
    public List<Guid> AdditionalServiceIds { get; set; }
}