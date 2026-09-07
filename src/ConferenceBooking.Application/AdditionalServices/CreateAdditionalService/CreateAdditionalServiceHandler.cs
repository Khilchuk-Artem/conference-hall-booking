using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.AdditionalServices.CreateAdditionalService;

public class CreateAdditionalServiceHandler : IRequestHandler<CreateAdditionalServiceCommand, Guid>
{
    private readonly IRepository<AdditionalService> _additionalServiceRepository;

    public CreateAdditionalServiceHandler(IRepository<AdditionalService> additionalServiceRepository)
    {
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<Guid> Handle(CreateAdditionalServiceCommand request, CancellationToken cancellationToken)
    {
        var additionalService = new AdditionalService
        {
            Name = request.Name,
            Price = request.Price
        };

        await _additionalServiceRepository.Add(additionalService);

        return additionalService.Id;
    }
}
