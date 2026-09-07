using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.GetAdditionalServiceById;

public class GetAdditionalServiceByIdQuery : IRequest<AdditionalServiceDto>
{
    public Guid Id { get; set; }
}
