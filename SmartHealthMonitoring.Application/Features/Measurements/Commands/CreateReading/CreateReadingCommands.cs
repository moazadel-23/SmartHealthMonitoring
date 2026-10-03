using MediatR;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Features.Measurements.Commands.CreateReading;

public class CreateReadingCommands : IRequest<Measurement>
{
    public int PatientId { get; set; }
    public double HeartRate { get; set; }
    public double SpO2 { get; set; }
}
