using MediatR;
using SmartHealthMonitoring.Application.DTOs;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.HeartRate.Queries.GetReadingForPatient;

public record GetReadingForPatientQuery(int PatientId) : IRequest<IEnumerable<HeartRateResponseDto>?>;
