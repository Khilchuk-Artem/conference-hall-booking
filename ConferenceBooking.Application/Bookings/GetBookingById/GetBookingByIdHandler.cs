using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Application.Specifications.Bookings;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.Bookings.GetBookingById;

public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    private readonly IRepository<Booking> _bookingRepository;

    public GetBookingByIdHandler(IRepository<Booking> bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new BookingWithAdditionalServicesSpecification();
        var booking = await _bookingRepository.GetById(request.Id, spec);

        if (booking == null) throw new NotFoundException("Booking", request.Id);

        return BookingMapper.ToDto(booking);
    }
}
