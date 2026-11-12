using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.API.Extensions;
using SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.Commands.CreateReading;
using SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.Queries.GetTempForPatient;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemperatureController : ControllerBase
{
    private readonly IMediator _mediator;

    public TemperatureController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Patient/{patientId}")]
    public async Task<IActionResult> GetPatientTemperature([FromRoute] int patientId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTempForPatientQuery(patientId), cancellationToken);

        if (result == null)
        {
            return NotFound(new { Message = $"Patient with ID {patientId} does not exist." });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReading(
        [FromBody] CreateTemperatureReadingCommands command,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult("Temperature", command.PatientId, "Temperature reading created successfully.");
    }
}
