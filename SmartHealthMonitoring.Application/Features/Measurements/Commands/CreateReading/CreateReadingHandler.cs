using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Features.Measurements.Commands.CreateReading;

public class CreateReadingHandler : IRequestHandler<CreateReadingCommands, Measurement>
{
    private readonly IMeasurementRepository _measurementRepository;

    public CreateReadingHandler(IMeasurementRepository measurementRepository)
    {
        _measurementRepository = measurementRepository;
    }

    public async Task<Measurement> Handle(CreateReadingCommands request, CancellationToken cancellationToken)
    {
        Measurement reading = new()
        {
            PatientId = request.PatientId,
            HeartRate = request.HeartRate,
            SpO2 = request.SpO2,
            RecordedAt = DateTime.UtcNow
        };

        await _measurementRepository.AddAsync(reading, cancellationToken);
        await _measurementRepository.SaveChangesAsync(cancellationToken);

        return reading;
    }
}
