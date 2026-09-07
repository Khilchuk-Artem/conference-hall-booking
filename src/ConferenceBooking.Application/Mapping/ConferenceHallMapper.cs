using System.Security;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace ConferenceBooking.Application.Mapping;

[Mapper]
public partial class ConferenceHallMapper
{
    [MapperIgnoreSource(nameof(ConferenceHall.Bookings))]
    public static partial ConferenceHallDto ToDto(ConferenceHall source);

    public static AdditionalServiceDto ToDto(AdditionalService source) => AdditionalServiceMapper.ToDto(source);

}
