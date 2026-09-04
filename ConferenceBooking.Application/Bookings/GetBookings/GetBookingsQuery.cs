using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.Bookings.GetBookings;

public class GetBookingsQuery : IRequest<List<BookingDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}