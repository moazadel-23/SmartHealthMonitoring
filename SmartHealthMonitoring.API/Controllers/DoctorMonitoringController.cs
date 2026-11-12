using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Doctor.DashBoard.LatestVitals.Queries.LatestVitalPatient;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api")]
[ApiController]
public class DoctorMonitoringController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorMonitoringController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{patientId}/LatestVitals")]
    public async Task<IActionResult> GetLatestVitals([FromRoute] int patientId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LatestVitalQuerry(patientId), cancellationToken);

        if (result == null)
        {
            return NotFound(new { Message = $"Patient with ID {patientId} does not exist." });
        }

        return Ok(result);
    }
}
