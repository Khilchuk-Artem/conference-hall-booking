using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.CreateAdditionalService;

public class CreateAdditionalServiceCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
