using Asp.Versioning;
using ConferenceBooking.Application.Reports.GetSummary;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReportsController : Controller
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary", Name = "GetReportSummary")]
    public async Task<IActionResult> GetSummary([FromQuery] GetReportSummaryQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
