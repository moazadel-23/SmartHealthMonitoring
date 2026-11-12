using MediatR;
using SmartHealthMonitoring.Application.DTOs;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Queries.GetPatient;

public record GetPatientQuery(int Id) : IRequest<PatientResponseDto?>;
