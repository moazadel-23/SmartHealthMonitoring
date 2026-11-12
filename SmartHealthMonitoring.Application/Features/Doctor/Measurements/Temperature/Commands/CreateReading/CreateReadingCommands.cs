using System.ComponentModel.DataAnnotations;
using SmartHealthMonitoring.Application.Common.Models;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.Commands.CreateReading;

public class CreateTemperatureReadingCommands : PatientBoundReadingCommandBase
{
    [Range(30.0, 45.0, ErrorMessage = "Temperature must be between 30.0 and 45.0 °C.")]
    public decimal TempValue { get; set; }
}
