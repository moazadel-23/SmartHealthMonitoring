using System.ComponentModel.DataAnnotations;
using SmartHealthMonitoring.Application.Common.Models;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.HeartRate.Commands.CreateReading;

public class CreateReadingCommands : PatientBoundReadingCommandBase
{
    [Range(0, 300, ErrorMessage = "HeartRateValue must be between 0 and 300 bpm.")]
    public double HeartRate { get; set; }

    [Range(0, 100, ErrorMessage = "SpO2 must be between 0% and 100%.")]
    public double SpO2 { get; set; }
}
