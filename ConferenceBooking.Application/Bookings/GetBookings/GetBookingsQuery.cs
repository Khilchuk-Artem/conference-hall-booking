using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.Bookings.GetBookings;

public class GetBookingsQuery : IRequest<List<BookingDto>>
{
    public int page { get; set; }
    public int pageSize { get; set; }
}