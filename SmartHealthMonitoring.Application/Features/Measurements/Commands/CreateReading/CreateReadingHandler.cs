using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Measurements.Commands.CreateReading;

public class CreateReadingHandler : IRequestHandler<CreateReadingCommands, bool?>
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

    public async Task<bool?> Handle(CreateReadingCommands request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetOne(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return false;
        }

        var reading = new Measurement
        {
            PatientId = request.PatientId,
            HeartRate = request.HeartRate,
            SpO2 = request.SpO2,
            RecordedAt = DateTime.Now
        };

        await _measurementRepository.AddAsync(reading, cancellationToken);
        await _measurementRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
