using MediatR;
using SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.Queries.GetTempForPatient;

public class GetTempForPatientHandler : IRequestHandler<GetTempForPatientQuery, IEnumerable<TemperatureResponseDto>?>
{
    private readonly ITemperatureRepository _temperatureRepository;
    private readonly IPatientRepository _patientRepository;

    public GetTempForPatientHandler(
        ITemperatureRepository temperatureRepository,
        IPatientRepository patientRepository)
    {
        _temperatureRepository = temperatureRepository;
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<TemperatureResponseDto>?> Handle(
        GetTempForPatientQuery request,
        CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetOne(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return null;
        }

        var readings = await _temperatureRepository.GetByPatientIdAsync(request.PatientId, cancellationToken);

        return readings.Select(t => new TemperatureResponseDto
        {
            Id = t.Id,
            PatientId = t.PatientId,
            TempValue = t.TempValue,
            RecordedAt = t.RecordedAt
        });
    }
}
