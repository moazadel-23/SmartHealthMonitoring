using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Interfaces;

public interface IMeasurementService
{
    Task<Measurement> CreateReading(ReadingDto readingDto);
}
