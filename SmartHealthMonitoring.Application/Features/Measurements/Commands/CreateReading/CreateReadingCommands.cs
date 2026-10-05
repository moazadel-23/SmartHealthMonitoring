using System.ComponentModel.DataAnnotations;
using MediatR;
using SmartHealthMonitoring.Application.DTOs;

namespace SmartHealthMonitoring.Application.Features.Measurements.Commands.CreateReading;

public class CreateReadingCommands : IRequest<bool?>
{
    [Range(1, int.MaxValue, ErrorMessage = "PatientId must be greater than 0.")]
    public int PatientId { get; set; }

    [Range(30, 250, ErrorMessage = "HeartRate must be between 30 and 250 bpm.")]
    public double HeartRate { get; set; }

    [Range(70, 100, ErrorMessage = "SpO2 must be between 70% and 100%.")]
    public double SpO2 { get; set; }
}
