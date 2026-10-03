using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Measurements.Commands.CreateReading;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeasurementController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeasurementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReading([FromBody] CreateReadingCommands command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return BadRequest(new { Message = $"Patient with ID {command.PatientId} does not exist." });
        }

        return Ok(result);
    }
}
