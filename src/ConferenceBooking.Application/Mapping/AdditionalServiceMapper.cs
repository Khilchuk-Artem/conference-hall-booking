using ConferenceBooking.Application.DTO;
using ConferenceBooking.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace ConferenceBooking.Application.Mapping;

[Mapper]
public partial class AdditionalServiceMapper
{
    public static partial AdditionalServiceDto ToDto(AdditionalService source);
}
