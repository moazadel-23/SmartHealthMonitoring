using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.CreatePatient;
using SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.DeletePatient;
using SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.UpdatePatient;
using SmartHealthMonitoring.Application.Features.Reception.Patients.Queries.GetAllPatients;
using SmartHealthMonitoring.Application.Features.Reception.Patients.Queries.GetPatient;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPatients()
    {
        var result = await _mediator.Send(new GetAllPatientsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var result = await _mediator.Send(new GetPatientQuery(id));

        if (result == null)
        {
            return NotFound(new { Message = $"Patient with ID {id} does not exist." });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommands command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return BadRequest(new { Message = $"Doctor with ID {command.DoctorId} does not exist." });
        }

        return Ok(new
        {
            msg = "Patient created successfully",
            id = result.Value
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientCommands command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        command.Id = id;
        var result = await _mediator.Send(command);

        return result switch
        {
            UpdatePatientResult.PatientNotFound => NotFound(new { Message = $"Patient with ID {id} does not exist." }),
            UpdatePatientResult.DoctorNotFound => BadRequest(new { Message = $"Doctor with ID {command.DoctorId} does not exist." }),
            _ => Ok(new { msg = "Patient updated successfully" })
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var result = await _mediator.Send(new DeletePatientCommands(id));

        return result switch
        {
            DeletePatientResult.PatientNotFound => NotFound(new { Message = $"Patient with ID {id} does not exist." }),
            DeletePatientResult.PatientCurrentlySelected => Conflict(new { Message = $"Cannot delete patient with ID {id} because they are currently selected for active monitoring. Please select another patient first." }),
            _ => Ok(new { msg = "Patient deleted successfully" })
        };
    }
}
