using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.DTO;
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
        
        if (entity == null) return null;

        var res = await _conferenceHallRepository.Delete(request.Id);
        
        if (res == null) return null;

        return ConferenceHallMapper.ToDto(res);
    }
}