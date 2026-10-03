using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;

namespace SmartHealthMonitoring.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeasurementController : ControllerBase
{
    private readonly IMeasurementService _measurementService;
    private readonly IPatientRepository _patientRepository;

    public MeasurementController(IMeasurementService measurementService, IPatientRepository patientRepository)
    {
        _measurementService = measurementService;
        _patientRepository = patientRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReading([FromBody] ReadingDto readingDto)
    {
        var patientExists = await _patientRepository.GetOne(readingDto.PatientId);

        if (patientExists == null)
        {
            return BadRequest("Patient does not exist.");
        }

        var result = await _measurementService.CreateReading(readingDto);
        return Ok(result);
    }
}
