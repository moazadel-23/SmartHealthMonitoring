using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Reception.Doctors.Commands.CreateDoctor;
using SmartHealthMonitoring.Application.Features.Reception.Doctors.Commands.DeleteDoctor;
using SmartHealthMonitoring.Application.Features.Reception.Doctors.Commands.UpdateDoctor;
using SmartHealthMonitoring.Application.Features.Reception.Doctors.Queries.GetAllDoctors;
using SmartHealthMonitoring.Application.Features.Reception.Doctors.Queries.GetDoctor;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctors()
    {
        var result = await _mediator.Send(new GetAllDoctorsQuery());
        return Ok(result);
    }
   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var result = await _mediator.Send(new GetDoctorQuery(id));

        if (result == null)
        {
            return NotFound(new { Message = $"Doctor with ID {id} does not exist." });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommands command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(command);

        return Ok(new
        {
            msg = "Doctor created successfully"
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorCommands command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        command.Id = id;
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new { Message = $"Doctor with ID {id} does not exist." });
        }

        return Ok(new
        {
            msg = "Doctor updated successfully"
        }); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var result = await _mediator.Send(new DeleteDoctorCommands(id));

        if (!result)
        {
            return NotFound(new { Message = $"Doctor with ID {id} does not exist." });
        }

        return Ok(new { msg = "Doctor deleted successfully" });
    }
}
