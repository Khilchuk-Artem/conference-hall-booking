using ConferenceBooking.Application.DTO;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.EditAdditionalService;

public class EditAdditionalServiceCommand : IRequest<AdditionalServiceDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
