using ConferenceBooking.Application.DTO;
using ConferenceBooking.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace ConferenceBooking.Application.Mapping;

[Mapper]
public partial class BookingMapper
{
    public static partial BookingDto ToDto(Booking source);
    public static partial BookingServiceDto ToDto(BookingService source);

}