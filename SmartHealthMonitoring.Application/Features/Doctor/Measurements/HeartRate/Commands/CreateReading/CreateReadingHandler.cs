using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Enums;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.HeartRate.Commands.CreateReading;

public class CreateReadingHandler : IRequestHandler<CreateReadingCommands, CreateReadingResult>
{
    private readonly IHeartRateRepository _heartRateRepository;

    public CreateReadingHandler(IHeartRateRepository heartRateRepository)
    {
        _heartRateRepository = heartRateRepository;
    }

    public async Task<CreateReadingResult> Handle(CreateReadingCommands request, CancellationToken cancellationToken)
    {
        var reading = new Domain.Entities.HeartRate
        {
            PatientId = request.ResolvedPatientId,
            HeartRateValue = request.HeartRate,
            SpO2 = request.SpO2,
        };

        await _heartRateRepository.AddAsync(reading, cancellationToken);
        await _heartRateRepository.SaveChangesAsync(cancellationToken);

        return CreateReadingResult.Success;
    }
}
