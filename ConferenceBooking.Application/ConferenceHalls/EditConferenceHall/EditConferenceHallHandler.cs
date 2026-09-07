using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Application.Specifications.AdditionalServices;
using ConferenceBooking.Application.Specifications.ConferenceHalls;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.EditConferenceHall;

public class EditConferenceHallHandler : IRequestHandler<EditConferenceHallCommand, ConferenceHallDto>
{
    private readonly IRepository<ConferenceHall> _conferenceHallRepository;
    private readonly IRepository<AdditionalService> _additionalServiceRepository;

    public EditConferenceHallHandler(IRepository<ConferenceHall> conferenceHallRepository, IRepository<AdditionalService> additionalServiceRepository)
    {
        _conferenceHallRepository = conferenceHallRepository;
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<ConferenceHallDto> Handle(EditConferenceHallCommand request, CancellationToken cancellationToken)
    {
        var hallWithServicesSpec = new ConferenceHallWithServicesSpecification();
        var entity = await _conferenceHallRepository.GetById(request.Id, hallWithServicesSpec);
        
        if (entity == null) throw new NotFoundException("Conference hall", request.Id);
        
        entity.Name = request.Name;
        entity.Capacity = request.Capacity;
        entity.RentRate = request.RentRate;

        var servicesFromListSpec = new AdditionalServicesFromListSpecification(request.AdditionalServiceIds);

        var additionalServices = await _additionalServiceRepository.GetAll(servicesFromListSpec);
        
        if (additionalServices.Count != request.AdditionalServiceIds.Distinct().Count()) throw new BadRequestException("One or more additional services were not found.");
        
        entity.AdditionalServices.Clear();
        entity.AdditionalServices.AddRange(additionalServices);
        
        await _conferenceHallRepository.Update(entity);
        
        return ConferenceHallMapper.ToDto(entity);
    }
}
