using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Services;

public class MeasurementService : IMeasurementService
{
    private readonly IMeasurementRepository _measurementRepository;

    public MeasurementService(IMeasurementRepository measurementRepository)
    {
        _measurementRepository = measurementRepository;
    }

    public async Task<Measurement> CreateReading(ReadingDto readingDto)
    {
        Measurement reading = new()
        {
            PatientId = readingDto.PatientId,
            HeartRate = readingDto.HeartRate,
            SpO2 = readingDto.SpO2,
            RecordedAt = DateTime.UtcNow
        };

        await _measurementRepository.AddAsync(reading);
        await _measurementRepository.SaveChangesAsync();

        return reading;
    }
}
