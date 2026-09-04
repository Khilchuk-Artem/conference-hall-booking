using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Application.Specifications.Bookings;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.Bookings.GetBookings;

public class GetBookingsHandler : IRequestHandler<GetBookingsQuery, List<BookingDto>>
{
    private readonly IRepository<Booking> _bookingRepository;

    public GetBookingsHandler(IRepository<Booking> bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<List<BookingDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var spec = new BookingWithAdditionalServicesSpecification(request.page, request.pageSize);
        var bookings = await _bookingRepository.GetAll(spec);
        
        var results = bookings.Select(b => BookingMapper.ToDto(b)).ToList();
        
        return results;
    }
}