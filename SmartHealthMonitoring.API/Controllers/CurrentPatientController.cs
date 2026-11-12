using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Doctor.CurrentPatient.Commands.SetCurrentPatient;
using SmartHealthMonitoring.Application.Features.Doctor.CurrentPatient.Queries.GetCurrentPatient;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api/current-patient")]
[ApiController]
public class CurrentPatientController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrentPatientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentPatient(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentPatientQuery(), cancellationToken);

        if (result == null)
        {
            return NotFound(new { Message = "No patient is currently selected." });
        }

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> SetCurrentPatient(
        [FromBody] SetCurrentPatientCommand command,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);

        if (result.Status == SetCurrentPatientResult.PatientNotFound || result.CurrentPatient == null)
        {
            return NotFound(new { Message = $"Patient with ID {command.PatientId} does not exist." });
        }

        return Ok(new
        {
            Message = "Current patient selected successfully.",
            result.CurrentPatient.PatientId,
            result.CurrentPatient.PatientName,
            result.CurrentPatient.SelectedAt,
            result.CurrentPatient.SelectionVersion
        });
    }
}
