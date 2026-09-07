using Asp.Versioning;
using ConferenceBooking.Api.Idempotency;
using ConferenceBooking.Application.ConferenceHalls.CreateConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.DeleteConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.EditConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.GetAvailableConferenceHalls;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ConferenceHallsController : Controller
{
    private readonly IMediator _mediator;

    public ConferenceHallsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateConferenceHall")]
    [Idempotency]
    public async Task<IActionResult> Create([FromBody] CreateConferenceHallCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Create), new { version = "1", id }, id);
    }

    [HttpGet("available", Name = "GetAvailableConferenceHalls")]
    public async Task<IActionResult> GetAvailable([FromQuery] GetAvailableConferenceHallsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}", Name = "EditConferenceHall")]
    public async Task<IActionResult> Edit(Guid id, [FromBody] EditConferenceHallCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}", Name = "DeleteConferenceHall")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteConferenceHallCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}
