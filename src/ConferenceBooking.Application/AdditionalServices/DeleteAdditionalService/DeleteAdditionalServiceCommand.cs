using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.DeleteAdditionalService;

public class DeleteAdditionalServiceCommand : IRequest<AdditionalServiceDto>
{
    public Guid Id { get; set; }
}
