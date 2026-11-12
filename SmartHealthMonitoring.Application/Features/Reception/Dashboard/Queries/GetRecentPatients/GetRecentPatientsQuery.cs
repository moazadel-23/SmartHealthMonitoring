using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using System.Collections.Generic;

namespace SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetRecentPatients
{
    public record GetRecentPatientsQuery(int Count = 3) : IRequest<IEnumerable<PatientResponseDto>>;
}
