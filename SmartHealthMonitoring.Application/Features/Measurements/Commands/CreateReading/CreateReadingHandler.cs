using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Features.Measurements.Commands.CreateReading;

public class CreateReadingHandler : IRequestHandler<CreateReadingCommands, MeasurementResponseDto?>
{
    private readonly IMeasurementRepository _measurementRepository;
    private readonly IPatientRepository _patientRepository;

    public CreateReadingHandler(
        IMeasurementRepository measurementRepository,
        IPatientRepository patientRepository)
    {
        _measurementRepository = measurementRepository;
        _patientRepository = patientRepository;
    }

    public async Task<MeasurementResponseDto?> Handle(CreateReadingCommands request, CancellationToken cancellationToken)
    {
        // Business Logic: Verify Patient existence before inserting measurement
        var patient = await _patientRepository.GetOne(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return null;
        }

        var reading = new Measurement
        {
            PatientId = request.PatientId,
            HeartRate = request.HeartRate,
            SpO2 = request.SpO2,
            RecordedAt = DateTime.UtcNow
        };

        await _measurementRepository.AddAsync(reading, cancellationToken);
        await _measurementRepository.SaveChangesAsync(cancellationToken);

        return new MeasurementResponseDto
        {
            Id = reading.Id,
            PatientId = reading.PatientId,
            HeartRate = reading.HeartRate,
            SpO2 = reading.SpO2,
            RecordedAt = reading.RecordedAt
        };
    }
}
