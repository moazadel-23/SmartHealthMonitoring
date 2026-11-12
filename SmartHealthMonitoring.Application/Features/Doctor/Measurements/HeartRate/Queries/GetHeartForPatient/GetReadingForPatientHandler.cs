using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Features.Doctor.Measurements.HeartRate.Queries.GetReadingForPatient;
using SmartHealthMonitoring.Application.Interfaces;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.HeartRate.Queries.GetHeartForPatient;

public class GetReadingForPatientHandler : IRequestHandler<GetReadingForPatientQuery, IEnumerable<HeartRateResponseDto>?>
{
    private readonly IHeartRateRepository _HeartRateRepository;
    private readonly IPatientRepository _patientRepository;

    public GetReadingForPatientHandler(
        IHeartRateRepository HeartRateRepository,
        IPatientRepository patientRepository)
    {
        _HeartRateRepository = HeartRateRepository;
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<HeartRateResponseDto>?> Handle(
        GetReadingForPatientQuery request,
        CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetOne(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return null;
        }

        var readings = await _HeartRateRepository.GetByPatientIdAsync(request.PatientId, cancellationToken);

        return readings.Select(m => new HeartRateResponseDto
        {
            Id = m.Id,
            PatientId = m.PatientId,
            HeartRate = m.HeartRateValue,
            SpO2 = m.SpO2,
            RecordedAt = m.RecordedAt
        });
    }
}
