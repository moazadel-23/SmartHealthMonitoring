using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using System.Collections.Generic;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Queries.GetAllPatients;

public record GetAllPatientsQuery() : IRequest<IEnumerable<PatientResponseDto>>;
