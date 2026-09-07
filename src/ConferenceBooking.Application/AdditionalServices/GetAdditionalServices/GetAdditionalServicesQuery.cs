using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.GetAdditionalServices;

public class GetAdditionalServicesQuery : IRequest<List<AdditionalServiceDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}
