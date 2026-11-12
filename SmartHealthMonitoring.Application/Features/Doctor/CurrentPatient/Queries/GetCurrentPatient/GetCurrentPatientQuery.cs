using MediatR;
using SmartHealthMonitoring.Application.DTOs;

namespace SmartHealthMonitoring.Application.Features.Doctor.CurrentPatient.Queries.GetCurrentPatient;

public record GetCurrentPatientQuery : IRequest<CurrentPatientResponseDto?>;
