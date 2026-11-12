using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Reception.Nurses.Commands.CreateNurse;
using SmartHealthMonitoring.Application.Features.Reception.Nurses.Commands.DeleteNurse;
using SmartHealthMonitoring.Application.Features.Reception.Nurses.Commands.UpdateNurse;
using SmartHealthMonitoring.Application.Features.Reception.Nurses.Queries.GetAllNurses;
using SmartHealthMonitoring.Application.Features.Reception.Nurses.Queries.GetNurse;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NurseController : ControllerBase
{
    private readonly IMediator _mediator;

    public NurseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNurses()
    {
        var result = await _mediator.Send(new GetAllNursesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNurseById(int id)
    {
        var result = await _mediator.Send(new GetNurseQuery(id));

        if (result == null)
        {
            return NotFound(new { Message = $"Nurse with ID {id} does not exist." });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNurse([FromBody] CreateNurseCommands command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(command);

        return Ok(new
        {
            msg = "Nurse created successfully"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNurse(int id, [FromBody] UpdateNurseCommands command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        command.Id = id;
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new { Message = $"Nurse with ID {id} does not exist." });
        }

        return Ok(new
        {
            msg = "Nurse updated successfully"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNurse(int id)
    {
        var result = await _mediator.Send(new DeleteNurseCommands(id));

        if (!result)
        {
            return NotFound(new { Message = $"Nurse with ID {id} does not exist." });
        }

        return Ok(new { msg = "Nurse deleted successfully" });
    }
}
