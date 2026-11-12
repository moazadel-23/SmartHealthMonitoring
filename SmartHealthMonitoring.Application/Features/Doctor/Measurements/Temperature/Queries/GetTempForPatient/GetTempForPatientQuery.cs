using MediatR;
using SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.DTOs;
using System.Collections.Generic;

namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.Queries.GetTempForPatient;

public record GetTempForPatientQuery(int PatientId) : IRequest<IEnumerable<TemperatureResponseDto>?>;
