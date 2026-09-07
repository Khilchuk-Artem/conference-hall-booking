using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Domain.Entities;
using MediatR;

namespace ConferenceBooking.Application.ConferenceHalls.DeleteConferenceHall;

public class DeleteConferenceHallHandler : IRequestHandler<DeleteConferenceHallCommand, ConferenceHallDto>
{
    private readonly IRepository<ConferenceHall> _conferenceHallRepository;

    public DeleteConferenceHallHandler(IRepository<ConferenceHall> conferenceHallRepository)
    {
        _conferenceHallRepository = conferenceHallRepository;
    }

    public async Task<ConferenceHallDto> Handle(DeleteConferenceHallCommand request, CancellationToken cancellationToken)
    {
        var entity = await _conferenceHallRepository.GetById(request.Id);
        
        if (entity == null) throw new NotFoundException("Conference hall", request.Id);

        var res = await _conferenceHallRepository.Delete(request.Id);
        
        if (res == null) throw new NotFoundException("Conference hall", request.Id);

        return ConferenceHallMapper.ToDto(res);
    }
}
