using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.API.Extensions;
using SmartHealthMonitoring.Application.Features.Doctor.Measurements.HeartRate.Commands.CreateReading;
using SmartHealthMonitoring.Application.Features.Doctor.Measurements.HeartRate.Queries.GetReadingForPatient;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api")]
[ApiController]
public class HeartRateController : ControllerBase
{
    private readonly IMediator _mediator;

    public HeartRateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Patient/{patientId}/heart-rate")]
    public async Task<IActionResult> GetPatientHeartRate([FromRoute] int patientId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReadingForPatientQuery(patientId), cancellationToken);

        if (result == null)
        {
            return NotFound(new { Message = $"Patient with ID {patientId} does not exist." });
        }

        return Ok(result);
    }

    [HttpPost("Heart-Rate")]
    public async Task<IActionResult> CreateReading(
        [FromBody] CreateReadingCommands command,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult("HeartRate", command.PatientId, "HeartRate Created Successfully");
    }
}
