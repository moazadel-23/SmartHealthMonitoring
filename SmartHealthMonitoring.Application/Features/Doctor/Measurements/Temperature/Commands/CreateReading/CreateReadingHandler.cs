using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Enums;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.Commands.CreateReading;

public class CreateTemperatureReadingHandler : IRequestHandler<CreateTemperatureReadingCommands, CreateReadingResult>
{
    private readonly ITemperatureRepository _temperatureRepository;

    public CreateTemperatureReadingHandler(ITemperatureRepository temperatureRepository)
    {
        _temperatureRepository = temperatureRepository;
    }

    public async Task<CreateReadingResult> Handle(CreateTemperatureReadingCommands request, CancellationToken cancellationToken)
    {
        var reading = new Domain.Entities.Temperature
        {
            PatientId = request.ResolvedPatientId,
            TempValue = request.TempValue,
            RecordedAt = DateTime.UtcNow,
        };

        await _temperatureRepository.AddAsync(reading, cancellationToken);
        await _temperatureRepository.SaveChangesAsync(cancellationToken);

        return CreateReadingResult.Success;
    }
}
