using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.Bookings.GetBookingById;

public class GetBookingByIdQuery : IRequest<BookingDto>
{
    public Guid Id { get; set; }
}
