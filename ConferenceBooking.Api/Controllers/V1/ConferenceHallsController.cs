using Asp.Versioning;
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
    public async Task<IActionResult> Create([FromBody] CreateConferenceHallCommand command)
    {
        var id = await _mediator.Send(command);
        if (id == Guid.Empty) return BadRequest("Conference hall was not created.");

        return CreatedAtAction(nameof(Create), new { version = "1", id }, id);
    }

    [HttpGet("available", Name = "GetAvailableConferenceHalls")]
    public async Task<IActionResult> GetAvailable([FromQuery] GetAvailableConferenceHallsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id:guid}", Name = "EditConferenceHall")]
    public async Task<IActionResult> Edit(Guid id, [FromBody] EditConferenceHallCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result is null ? BadRequest("Conference hall was not updated.") : Ok(result);
    }

    [HttpDelete("{id:guid}", Name = "DeleteConferenceHall")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteConferenceHallCommand { Id = id });
        return result is null ? NotFound() : NoContent();
    }
}
