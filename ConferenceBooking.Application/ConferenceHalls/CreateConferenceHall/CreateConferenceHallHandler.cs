using Ardalis.Specification;
using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Specifications.AdditionalServices;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.CreateConferenceHall;

public class CreateConferenceHallHandler : IRequestHandler<CreateConferenceHallCommand, Guid>
{
    private readonly IRepository<ConferenceHall> _conferenceHallRepository;
    private readonly IRepository<AdditionalService> _additionalServiceRepository;

    public CreateConferenceHallHandler(IRepository<ConferenceHall> conferenceHallRepository, IRepository<AdditionalService> additionalServiceRepository)
    {
        _conferenceHallRepository = conferenceHallRepository;
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<Guid> Handle(CreateConferenceHallCommand request, CancellationToken cancellationToken)
    {
        var conferenceHall = new ConferenceHall()
        {
            Name = request.Name,
            Capacity = request.Capacity,
            RentRate = request.RentRate,
        };
        
        var srevicesFromListSpec = new AdditionalServicesFromListSpecification(request.AdditionalServiceIds);
        
        var additionalServices = await _additionalServiceRepository.GetAll(srevicesFromListSpec);
        
        if (additionalServices.Count != request.AdditionalServiceIds.Distinct().Count()) return Guid.Empty; // throw exception later
        
        conferenceHall.AdditionalServices = additionalServices;
        
        await _conferenceHallRepository.Add(conferenceHall);
        
        return conferenceHall.Id;
    }
}