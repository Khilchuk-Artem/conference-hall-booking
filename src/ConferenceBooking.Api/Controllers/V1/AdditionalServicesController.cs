using Asp.Versioning;
using ConferenceBooking.Api.Idempotency;
using ConferenceBooking.Application.AdditionalServices.CreateAdditionalService;
using ConferenceBooking.Application.AdditionalServices.DeleteAdditionalService;
using ConferenceBooking.Application.AdditionalServices.EditAdditionalService;
using ConferenceBooking.Application.AdditionalServices.GetAdditionalServiceById;
using ConferenceBooking.Application.AdditionalServices.GetAdditionalServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdditionalServicesController : Controller
{
    private readonly IMediator _mediator;

    public AdditionalServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateAdditionalService")]
    [Idempotency]
    public async Task<IActionResult> Create([FromBody] CreateAdditionalServiceCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { version = "1", id }, id);
    }

    [HttpGet("{id:guid}", Name = "GetAdditionalServiceById")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAdditionalServiceByIdQuery { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpGet(Name = "GetAdditionalServices")]
    public async Task<IActionResult> Get([FromQuery] GetAdditionalServicesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}", Name = "EditAdditionalService")]
    public async Task<IActionResult> Edit(Guid id, [FromBody] EditAdditionalServiceCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}", Name = "DeleteAdditionalService")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAdditionalServiceCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}
